using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.CSharp; // You can remove this if you don't need dynamic type in .NET Standard frends Tasks
using Newtonsoft.Json;
using SendSafely.Objects;
using api = SendSafely;

#pragma warning disable 1591

namespace Frends.Community.SendSafely
{
    public class SendSafelyTasks
    {
        /// <summary>
        /// This task uses the SendSafely Windows Client API methods to perform SendSafely REST API operations. The methods are documented in https://developer.sendsafely.com/windows/ClientApi.htm.
        /// Documentation: https://github.com/CommunityHiQ/Frends.Community.SendSafely
        /// </summary>

        private List<UploadFilesResult> newEntries = new List<UploadFilesResult>();
        private List<DownloadFilesResult> downloadedFiles = new List<DownloadFilesResult>();
        private List<GetDirectoriesResult> directories = new List<GetDirectoriesResult>();
        private List<FileInfoWithDirInfo> fileInformation = new List<FileInfoWithDirInfo>();

        /// <summary>
        /// Downloads files from the given workspace directory and its subdirectories and decrypts them. If a date is given, downloads only files that are created or modified after the date.
        /// </summary>
        /// <returns>List [ Object { string Name, string Directory, string FullPath, string OrigName, string OrigDirectory, string OrigDirectoryId, DateTime CreationTime } ]</returns>
        public static List<DownloadFilesResult> DownloadFiles(Parameters parameters, DownloadFilesInput input, CancellationToken cancellationToken)
        {
            return new SendSafelyTasks().DownloadFilesNonStatic(parameters, input, cancellationToken);
        }

        private List<DownloadFilesResult> DownloadFilesNonStatic(Parameters parameters, DownloadFilesInput input, CancellationToken cancellationToken)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);

            if (string.IsNullOrEmpty(input.Date))
            {
                DownloadFilesRecursively(ssApi, input.PackageId, input.PackageKeyCode, input.DirectoryId, cancellationToken);
            }
            else
            {
                DateTime dateTime = DateTime.Parse(input.Date);
                DownloadFilesUploadedAfterDate(ssApi, input.PackageId, input.PackageKeyCode, dateTime, cancellationToken);
            }

            return downloadedFiles;
        }

        private void DownloadFilesRecursively(api.ClientAPI ssApi, string packageId, string keyCode, string dirId, CancellationToken cancellationToken)
        {
            api.Directory dir = ssApi.GetDirectory(packageId, dirId, 0, 0);

            foreach (var file in dir.Files)
            {
                cancellationToken.ThrowIfCancellationRequested();
                FileInfo newFile = ssApi.DownloadFileFromDirectory(packageId, dir.DirectoryId, file.FileId, keyCode, new ProgressCallback());
                DownloadFilesResult fileInfo = new DownloadFilesResult(newFile, file, dir);
                downloadedFiles.Add(fileInfo);
            }

            foreach (var subdir in dir.SubDirectories)
            {
                DownloadFilesRecursively(ssApi, packageId, keyCode, subdir.DirectoryId, cancellationToken);
            }
        }

        private void DownloadFilesUploadedAfterDate(api.ClientAPI ssApi, string packageId, string keyCode, DateTime date, CancellationToken cancellationToken)
        {
            api.PackageInformation pkgInfo = ssApi.GetPackageInformation(packageId);

            GetFileInformationRecursively(ssApi, pkgInfo.PackageId, pkgInfo.RootDirectoryId, cancellationToken);

            var newFiles = fileInformation.Where(f => f.uploaded > date);

            foreach (var file in newFiles)
            {
                FileInfo downloadedFile = ssApi.DownloadFileFromDirectory(packageId, file.directoryId, file.fileId, keyCode, new ProgressCallback());
                DownloadFilesResult fileInfo = new DownloadFilesResult(downloadedFile, file);
                downloadedFiles.Add(fileInfo);
            }
        }

        /// <summary>
        /// Encrypts and uploads files from a given directory and its subdirectories to the root directory of a Workspace package. Files will be encrypted before being uploaded to the server. Subdirectories will be created if they don't exist.
        /// </summary>
        /// <returns>List [ Object { string Name, string Id, bool IsFile } ]</returns>
        public static List<UploadFilesResult> UploadFiles(Parameters parameters, UploadFilesInput input, CancellationToken cancellationToken)
        {
            return new SendSafelyTasks().UploadFilesNonStatic(parameters, input, cancellationToken);
        }

        private List<UploadFilesResult> UploadFilesNonStatic(Parameters parameters, UploadFilesInput input, CancellationToken cancellationToken)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);
            api.PackageInformation pkgInfo = ssApi.GetPackageInformation(input.PackageId);
            api.Directory root = ssApi.GetDirectory(input.PackageId, pkgInfo.RootDirectoryId, 0, 0);

            UploadFilesRecursively(ssApi, input.Path, root, input.PackageId, input.PackageKeyCode, cancellationToken);

            return newEntries;
        }

        private void UploadFilesRecursively(api.ClientAPI ssApi, string localDirPath, api.Directory ssDir, string packageId, string keyCode, CancellationToken cancellationToken)
        {
            string[] files = System.IO.Directory.GetFiles(localDirPath, "*", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                cancellationToken.ThrowIfCancellationRequested();
                api.File addedFile = ssApi.EncryptAndUploadFileInDirectory(packageId, ssDir.DirectoryId, keyCode, file, new ProgressCallback());
                newEntries.Add(new UploadFilesResult(addedFile));
            }

            string[] subdirs = System.IO.Directory.GetDirectories(localDirPath, "*", SearchOption.TopDirectoryOnly);

            foreach (string subdir in subdirs)
            {
                api.Directory nextDir;
                var dirName = new DirectoryInfo(subdir).Name;

                // Check if directory exists
                var d = ssDir.SubDirectories.SingleOrDefault(s => s.Name == dirName);

                if (d != null)
                {
                    nextDir = ssApi.GetDirectory(packageId, d.DirectoryId, 0, 0);
                }
                else
                {
                    nextDir = ssApi.CreateDirectory(packageId, ssDir.DirectoryId, dirName);
                    newEntries.Add(new UploadFilesResult(nextDir));
                }

                UploadFilesRecursively(ssApi, subdir, nextDir, packageId, keyCode, cancellationToken);
            }
        }

        /// <summary>
        /// Retrieves meta data about a directory and its subdirectories in a workspace package.
        /// </summary>
        /// <returns>List [ Object { SendSafely.Directory UserDirectory, string DirectoryName, string DirectoryId, List&lt;FileResponse&gt; Files, Collection&lt;DirectoryResponse&gt; SubDirectories } ]</returns>
        public static List<GetDirectoriesResult> GetDirectories(Parameters parameters, GetDirectoriesInput input, CancellationToken cancellationToken)
        {
            return new SendSafelyTasks().GetDirectoriesNonStatic(parameters, input, cancellationToken);
        }

        private List<GetDirectoriesResult> GetDirectoriesNonStatic(Parameters parameters, GetDirectoriesInput input, CancellationToken cancellationToken)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);
            GetDirectoriesRecursively(ssApi, input.PackageId, input.RootDirectoryId, cancellationToken);
            return directories;
        }

        private void GetDirectoriesRecursively(api.ClientAPI ssApi, string packageId, string rootId, CancellationToken cancellationToken)
        {
            api.Directory dir = ssApi.GetDirectory(packageId, rootId, 0, 0);
            directories.Add(new GetDirectoriesResult(dir));

            foreach (var subdir in dir.SubDirectories)
            {
                cancellationToken.ThrowIfCancellationRequested();
                GetDirectoriesRecursively(ssApi, packageId, subdir.DirectoryId, cancellationToken);
            }
        }

         /// <summary>
        /// Retrieves meta data about all files in a Workspace package.
        /// </summary>
        /// <returns>List [ Object { string fileId, string fileName, string fileSize, string createdByEmail, string createdById, DateTime uploaded, string uploadedStr, string directoryName, string directoryId } ]</returns>
        public static List<GetFilesResult> GetFiles(Parameters parameters, GetFilesInput input, CancellationToken cancellationToken)
        {
            return new SendSafelyTasks().GetFilesNonStatic(parameters, input, cancellationToken);
        }

        private List<GetFilesResult> GetFilesNonStatic(Parameters parameters, GetFilesInput input, CancellationToken cancellationToken)
        {
            List<GetFilesResult> output = new List<GetFilesResult>();

            api.ClientAPI ssApi = InitializeApi(parameters);
            api.PackageInformation pkg = ssApi.GetPackageInformation(input.PackageId);

            GetFileInformationRecursively(ssApi, pkg.PackageId, pkg.RootDirectoryId, cancellationToken);

            foreach (var file in fileInformation)
            {
                output.Add(new GetFilesResult(file));
            }

            return output;
        }

        /// <summary>
        /// Fetches the latest package meta data about a specific package given the unique package id.
        /// </summary>
        /// <returns>Object { List&lt;ContactGroup&gt; ContactGroups, bool IsWorkspace, string PackageDescriptor, string RootDirectoryId, PackageStatus Status, string State, string PackageOwner, DateTime PackageTimestamp, int Life, List&lt;string&gt; Approvers, List&lt;SendSafely.File&gt; Files, List&lt;SendSafely.Recipient&gt; Recipients, bool NeedsApprover, string ServerSecret, string KeyCode, string PackageCode, string PackageId, bool AllowReplyAll, string PackageParentId }</returns>
        public static GetPackageInformationResult GetPackageInformation(Parameters parameters, GetPackageInformationInput input)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);
            api.PackageInformation pkgInfo = ssApi.GetPackageInformation(input.PackageId);

            return new GetPackageInformationResult(pkgInfo);
        }

        // To be implemented
        public static api.Directory CreateDirectory(Parameters parameters, CreateDirectoryInput input)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);
            api.Directory newDir = ssApi.CreateDirectory(input.PackageId, input.ParentDirectoryId, input.DirectoryName);

            return newDir;
        }

        // To be implemented
        public static void DeleteDirectory(Parameters parameters, DeleteDirectoryInput input)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);
            ssApi.DeleteDirectory(input.PackageId, input.DirectoryId);
        }

        // To be implemented
        public static void MoveDirectory(Parameters parameters, MoveDirectoryInput input)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);
            ssApi.MoveDirectory(input.PackageId, input.SourceDirectoryId, input.DestinationDirectoryId);
        }

        // To be implemented
        public static void RenameDirectory(Parameters parameters, RenameDirectoryInput input)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);
            ssApi.RenameDirectory(input.PackageId, input.DirectoryId, input.DirectoryName);
        }

        // To be implemented
        public static void DeleteFile(Parameters parameters, DeleteFileInput input)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);
            ssApi.DeleteFile(input.PackageId, input.DirectoryId, input.FileId);
        }

        // To be implemented
        public static void MoveFile(Parameters parameters, MoveFileInput input)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);
            ssApi.MoveFile(input.PackageId, input.FileId, input.DestinationDirectoryId);
        }

        // To be implemented
        public static List<ActivityLogEntry> GetActivityLog(Parameters parameters, GetActivityLogInput input)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);
            List<ActivityLogEntry> log = ssApi.GetActivityLog(input.PackageId, input.RowIndex);

            return log;
        }

        // To be implemented
        public static api.Recipient AddRecipient(Parameters parameters, AddRecipientInput input)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);
            api.Recipient recipient = ssApi.AddRecipient(input.PackageId, input.Email);

            return recipient;
        }

        // To be implemented
        public static string FinalizePackage(Parameters parameters, FinalizePackageInput input)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);
            string packageLink = ssApi.FinalizePackage(input.PackageId, input.Keycode);

            return packageLink;
        }

        // To be implemented
        public static string FinalizeUndisclosedPackage(Parameters parameters, FinalizeUndisclosedPackageInput input)
        {
            api.ClientAPI ssApi = InitializeApi(parameters);

            string packageLink;

            if (input.Password == null)
            {
                packageLink = ssApi.FinalizeUndisclosedPackage(input.PackageId, input.Keycode);
            }
            else
            {
                packageLink = ssApi.FinalizeUndisclosedPackage(input.PackageId, input.Password, input.Keycode);
            }

            return packageLink;
        }

        private static api.ClientAPI InitializeApi(Parameters parameters)
        {
            api.ClientAPI ssApi = new api.ClientAPI();
            ssApi.InitialSetup(parameters.BaseUrl, parameters.ApiKey, parameters.ApiSecret);
            string userEmail = ssApi.VerifyCredentials();
            return ssApi;
        }

        private void GetFileInformationRecursively(api.ClientAPI ssApi, string packageId, string dirId, CancellationToken cancellationToken)
        {
            api.Directory dir = ssApi.GetDirectory(packageId, dirId, 0, 0);

            foreach (var file in dir.Files)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var ssFileInfo = ssApi.GetFileInformation(packageId, dirId, file.FileId);
                FileInfoWithDirInfo fileInfo = new FileInfoWithDirInfo(ssFileInfo, dir);
                fileInformation.Add(fileInfo);
            }

            foreach (var subdir in dir.SubDirectories)
            {
                GetFileInformationRecursively(ssApi, packageId, subdir.DirectoryId, cancellationToken);
            }
        }
    }
}
