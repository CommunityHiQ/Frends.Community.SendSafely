#pragma warning disable 1591

using api = SendSafely;
using SendSafely.Objects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Frends.Community.SendSafely
{
    #region Common parameters
    /// <summary>
    /// Parameters for creating connection to SendSafely API.
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// SendSafely base URL
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("https://app.sendsafely.com")]
        public string BaseUrl { get; set; }

        /// <summary>
        /// SendSafely API key
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string ApiKey { get; set; }

        /// <summary>
        /// SendSafely API secret
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        [PasswordPropertyText]
        public string ApiSecret { get; set; }
    }
    #endregion

    #region DownloadFilesTask
    /// <summary>
    /// Parameters for downloading files in a workspace directory.
    /// </summary>
    public class DownloadFilesInput
    {
        /// <summary>
        /// Workspace package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }

        /// <summary>
        /// Workspace package key code
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageKeyCode { get; set; }

        /// <summary>
        /// Directory ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string DirectoryId { get; set; }

        /// <summary>
        /// Optional, only files uploaded after this date will be downloaded. Use the format yyyy-mm-ddThh:mm:ss.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string Date { get; set; }
    }

    public class DownloadFilesResult
    {
        public string Name { get; set; }
        public string Directory { get; set; }
        public string FullPath { get; set; }
        public string OrigName { get; set; }
        public string OrigDirectory { get; set; }
        public string OrigDirectoryId { get; set; }
        public DateTime CreationTime { get; set; }

        public DownloadFilesResult(FileInfo fileInfo, FileResponse fileResponse, api.Directory directory)
        {
            this.Name = fileInfo.Name;
            this.Directory = fileInfo.DirectoryName;
            this.FullPath = fileInfo.FullName;
            this.OrigName = fileResponse.FileName;
            this.OrigDirectory = directory.DirectoryName;
            this.OrigDirectoryId = directory.DirectoryId;
            this.CreationTime = fileInfo.CreationTimeUtc;
        }

        public DownloadFilesResult(FileInfo fileInfo, FileInfoWithDirInfo fileInfoWithDirInfo)
        {
            this.Name = fileInfo.Name;
            this.Directory = fileInfo.DirectoryName;
            this.FullPath = fileInfo.FullName;
            this.OrigName = fileInfoWithDirInfo.fileName;
            this.OrigDirectory = fileInfoWithDirInfo.directoryName;
            this.OrigDirectoryId = fileInfoWithDirInfo.directoryId;
            this.CreationTime = fileInfo.CreationTimeUtc;
        }
    }
    #endregion

    #region UploadFilesTask
    /// <summary>
    /// Parameters for uploading files to an existing workspace in SendSafely.
    /// </summary>

    public class UploadFilesInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }

        /// <summary>
        /// Package key code
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageKeyCode { get; set; }

        /// <summary>
        /// Path to the directory that contains the files to be uploaded.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string Path { get; set; }
    }

    public class UploadFilesResult
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool IsFile { get; set; }
        public UploadFilesResult(api.Directory directory)
        {
            this.Name = directory.DirectoryName;
            this.Id = directory.DirectoryId;
            this.IsFile = false;
        }

        public UploadFilesResult(api.File file)
        {
            this.Name = file.FileName;
            this.Id = file.FileId;
            this.IsFile = true;
        }
    }
    #endregion

    #region GetDirectoriesTask
    /// <summary>
    /// Parameters for getting all the directories in a directory.
    /// </summary>

    public class GetDirectoriesInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }

        /// <summary>
        /// Root directory ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string RootDirectoryId { get; set; }
    }

    public class GetDirectoriesResult
    {
        public api.Directory UserDirectory { get; set; }
        public string DirectoryName { get; set; }
        public string DirectoryId { get; set; }
        public List<FileResponse> Files { get; set; }
        public Collection<DirectoryResponse> SubDirectories { get; set; }

        public GetDirectoriesResult(api.Directory directory)
        {
            this.DirectoryId = directory.DirectoryId;
            this.DirectoryName = directory.DirectoryName;
            this.UserDirectory = directory.UserDirectory;
            this.Files = directory.Files;
            this.SubDirectories = directory.SubDirectories;
        }
    }
    #endregion

    #region GetFilesTask
    /// <summary>
    /// Parameters for getting information about all files in a package.
    /// </summary>

    public class GetFilesInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }
    }

    public class GetFilesResult
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string CreatedByEmail { get; set; }
        public string CreatedById { get; set; }
        public DateTime Uploaded { get; set; }
        public string UploadedStr { get; set; }
        public string DirectoryName { get; set; }
        public string DirectoryId { get; set; }

        public GetFilesResult(FileInfoWithDirInfo fileInfoWithDirInfo)
        {
            this.FileName = fileInfoWithDirInfo.fileName;
            this.FileId = fileInfoWithDirInfo.fileId;
            this.FileSize = fileInfoWithDirInfo.fileSize;
            this.CreatedByEmail = fileInfoWithDirInfo.createdByEmail;
            this.CreatedById = fileInfoWithDirInfo.createdById;
            this.Uploaded = fileInfoWithDirInfo.uploaded;
            this.UploadedStr = fileInfoWithDirInfo.uploadedStr;
            this.DirectoryName = fileInfoWithDirInfo.directoryName;
            this.DirectoryId = fileInfoWithDirInfo.directoryId;
        }
    }
    #endregion

    #region GetPackageInformationTask
    /// <summary>
    /// Parameters for getting package information.
    /// </summary>

    public class GetPackageInformationInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }
    }

    public class GetPackageInformationFromLinkInput
    {
        /// <summary>
        /// Package link
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageLink { get; set; }
    }

    public class GetPackageInformationResult
    {
        //
        // Summary:
        //     A list of contact groups
        public List<ContactGroup> ContactGroups { get; set; }
        //
        // Summary:
        //     The flag specifying the package is a workspace.
        public bool IsWorkspace { get; set; }
        //
        // Summary:
        //     The package descriptor.
        public string PackageDescriptor { get; set; }
        //
        // Summary:
        //     The root directory of a workspace package.
        public string RootDirectoryId { get; set; }
        //
        // Summary:
        //     The status of the package.
        public PackageStatus Status { get; set; }
        //
        // Summary:
        //     The state of the pakage.
        public string State { get; set; }
        //
        // Summary:
        //     The Package Owner of the package.
        public string PackageOwner { get; set; }
        //
        // Summary:
        //     The timestamp of when the package was finalized.
        public DateTime PackageTimestamp { get; set; }
        //
        // Summary:
        //     The current package life. The package life determines for how long the package
        //     should be available to the recipients. It's measured in days.
        public int Life { get; set; }
        //
        // Summary:
        //     A list of approvers that are currently attached to the package.
        public List<string> Approvers { get; set; }
        //
        // Summary:
        //     A list of files that are currently attached to the package.
        public List<api.File> Files { get; set; }
        //
        // Summary:
        //     A list of recipients that are currently attached to the package.
        public List<api.Recipient> Recipients { get; set; }
        //
        // Summary:
        //     NeedsApprover will be true when a package needs to add at least one approver
        //     before the package can be finalized. If the package is finalized without the
        //     approver, an exception will be thrown.
        public bool NeedsApprover { get; set; }
        //
        // Summary:
        //     The server secret makes together with the keycode up the encryption key. The
        //     server secret is specific to a package and passed from the server.
        public string ServerSecret { get; set; }
        //
        // Summary:
        //     The keycode for the package. This key should always be kept client side and never
        //     be sent to the server. The keycode makes up one part of the encryption key.
        public string KeyCode { get; set; }
        //
        // Summary:
        //     The package code for the given package. The package code is a part of the URL
        //     that must be sent to the recipients.
        public string PackageCode { get; set; }
        //
        // Summary:
        //     The package ID for the given package.
        public string PackageId { get; set; }
        //
        // Summary:
        //     Allow reply all, false if BCC recipients
        public bool AllowReplyAll { get; set; }
        //
        // Summary:
        //     Package parent id
        public string PackageParentId { get; set; }

        public GetPackageInformationResult(api.PackageInformation pkgInfo)
        {
            this.ContactGroups = pkgInfo.ContactGroups;
            this.IsWorkspace = pkgInfo.IsWorkspace;
            this.PackageDescriptor = pkgInfo.PackageDescriptor;
            this.RootDirectoryId = pkgInfo.RootDirectoryId;
            this.Status = pkgInfo.Status;
            this.State = pkgInfo.State;
            this.PackageOwner = pkgInfo.PackageOwner;
            this.PackageTimestamp = pkgInfo.PackageTimestamp;
            this.Life = pkgInfo.Life;
            this.Approvers = pkgInfo.Approvers;
            this.Files = pkgInfo.Files;
            this.Recipients = pkgInfo.Recipients;
            this.NeedsApprover = pkgInfo.NeedsApprover;
            this.ServerSecret = pkgInfo.ServerSecret;
            this.KeyCode = pkgInfo.KeyCode;
            this.PackageCode = pkgInfo.PackageCode;
            this.PackageId = pkgInfo.PackageId;
            this.AllowReplyAll = pkgInfo.AllowReplyAll;
            this.PackageParentId = pkgInfo.PackageParentId;
        }
    }
    #endregion

    #region CreateDirectoryTask
    /// <summary>
    /// Parameters for creating a new directory to a workspace package.
    /// </summary>

    public class CreateDirectoryInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }

        /// <summary>
        /// Parent directory ID of the directory to be created
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string ParentDirectoryId { get; set; }

        /// <summary>
        /// Directory name
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string DirectoryName { get; set; }
    }

    public class CreateDirectoryResult
    {
        public api.Directory UserDirectory { get; set; }
        public string DirectoryName { get; set; }
        public string DirectoryId { get; set; }
        public List<FileResponse> Files { get; set; }
        public Collection<DirectoryResponse> SubDirectories { get; set; }

        public CreateDirectoryResult(api.Directory directory)
        {
            this.DirectoryId = directory.DirectoryId;
            this.DirectoryName = directory.DirectoryName;
            this.UserDirectory = directory.UserDirectory;
            this.Files = directory.Files;
            this.SubDirectories = directory.SubDirectories;
        }
    }
    #endregion

    #region DeleteDirectoryTask
    /// <summary>
    /// Parameters for deleting a directory from a workspace package.
    /// </summary>

    public class DeleteDirectoryInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }

        /// <summary>
        /// Directory ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string DirectoryId { get; set; }
    }
    #endregion

    #region MoveDirectoryTask
    /// <summary>
    /// Parameters for moving a directory in a workspace package.
    /// </summary>

    public class MoveDirectoryInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }

        /// <summary>
        /// Source directory ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string SourceDirectoryId { get; set; }

        /// <summary>
        /// Destination directory ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string DestinationDirectoryId { get; set; }
    }
    #endregion

    #region RenameDirectoryTask
    /// <summary>
    /// Parameters for renaming a directory in a workspace package.
    /// </summary>

    public class RenameDirectoryInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }

        /// <summary>
        /// Directory ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string DirectoryId { get; set; }

        /// <summary>
        /// New name for the directory
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string DirectoryName { get; set; }
    }
    #endregion

    #region DeleteFileTask
    /// <summary>
    /// Parameters for deleting a file from a workspace package.
    /// </summary>

    public class DeleteFileInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }

        /// <summary>
        /// Directory ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string DirectoryId { get; set; }

        /// <summary>
        /// File ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string FileId { get; set; }
    }
    #endregion

    #region MoveFileTask
    /// <summary>
    /// Parameters for moving a file to the specified destination directory in a workspace package.
    /// </summary>

    public class MoveFileInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }

        /// <summary>
        /// File ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string FileId { get; set; }

        /// <summary>
        /// Destination directory ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string DestinationDirectoryId { get; set; }

    }
    #endregion

    #region GetActivityLogTask
    /// <summary>
    /// Parameters for getting activity log.
    /// </summary>

    public class GetActivityLogInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }

        /// <summary>
        /// Row index
        /// </summary>
        [DefaultValue(0)]
        public int RowIndex { get; set; }
    }
    #endregion

    #region AddRecipientTask
    /// <summary>
    /// Parameters for adding a recipient to a package.
    /// </summary>

    public class AddRecipientInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }

        /// <summary>
        /// Email of the recipient
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string Email { get; set; }
    }
    #endregion

    #region FinalizePackageTask
    /// <summary>
    /// Parameters for finalizing a package.
    /// </summary>

    public class FinalizePackageInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }

        /// <summary>
        /// Key code
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string Keycode { get; set; }
    }
    #endregion

    #region FinalizeUndisclosedPackageTask
    /// <summary>
    /// Parameters for finalizing a package.
    /// </summary>

    public class FinalizeUndisclosedPackageInput
    {
        /// <summary>
        /// Package ID
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string PackageId { get; set; }

        /// <summary>
        /// Key code
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string Keycode { get; set; }

        /// <summary>
        /// Password, optional
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string Password { get; set; }
    }
    #endregion
}
