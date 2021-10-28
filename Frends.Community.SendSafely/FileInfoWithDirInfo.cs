#pragma warning disable 1591

using System;
using api = SendSafely;
using SendSafely.Objects;

namespace Frends.Community.SendSafely
{
    public class FileInfoWithDirInfo
    {
        public string fileId { get; set; }
        public string fileName { get; set; }
        public string fileSize { get; set; }
        public string createdByEmail { get; set; }
        public string createdById { get; set; }
        public DateTime uploaded { get; set; }
        public string uploadedStr { get; set; }
        public string directoryName { get; set; }
        public string directoryId { get; set; }

        public FileInfoWithDirInfo(FileInformation fileInfo, api.Directory directory)
        {
            this.fileName = fileInfo.FileName;
            this.fileId = fileInfo.FileId;
            this.fileSize = fileInfo.FileSize;
            this.createdByEmail = fileInfo.CreatedByEmail;
            this.createdById = fileInfo.CreatedById;
            this.uploaded = fileInfo.Uploaded;
            this.uploadedStr = fileInfo.UploadedStr;
            this.directoryName = directory.DirectoryName;
            this.directoryId = directory.DirectoryId;
        }
    }
}

