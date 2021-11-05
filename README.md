# Frends.Community.SendSafely

frends Community Task for SendSafelyTasks

[![Actions Status](https://github.com/CommunityHiQ/Frends.Community.SendSafely/workflows/PackAndPushAfterMerge/badge.svg)](https://github.com/CommunityHiQ/Frends.Community.SendSafely/actions) ![MyGet](https://img.shields.io/myget/frends-community/v/Frends.Community.SendSafely) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) 

- [Installing](#installing)
- [Tasks](#tasks)
     - [SendSafelyTasks](#SendSafelyTasks)
- [Building](#building)
- [Contributing](#contributing)
- [Change Log](#change-log)

# Installing

You can install the Task via frends UI Task View or you can find the NuGet package from the following NuGet feed
https://www.myget.org/F/frends-community/api/v3/index.json and in Gallery view in MyGet https://www.myget.org/feed/frends-community/package/nuget/Frends.Community.SendSafely

# Tasks

## DownloadFiles

Downloads files from the given Workspace directory and its subdirectories and decrypts them. If a date is given, downloads only files that are created or modified after the date.

### Properties

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| BaseUrl | `string` | SendSafely base URL | `https://app.sendsafely.com` |
| ApiKey | `string` | SendSafely API key |  |
| ApiSecret | `string` | SendSafely API secret |  |
| PackageId | `string` | Workspace package ID |  |
| PackageKeyCode | `string` | Workspace package key code |  |
| DirectoryId | `string` | ID of the Workspace directory containing the files to be downloaded |  |
| Date | `string` | Optional, only files uploaded after this date will be downloaded. Use the format yyyy-mm-ddThh:mm:ss. | `2021-10-22T00:00:00` |

### Returns

Returns a list of objects with the following parameters.

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| Name | `string` | Name of the downloaded file. | `e6b609fe-ec72-4754-a496-7d225aa5fbc6` |
| Directory | `string` | Path of the destination directory. | `C:\\Users\\user\\AppData\\Local\\Temp` |
| FullPath | `string` | Full path to the downloaded file. | `C:\\Users\\user\\AppData\\Local\\Temp\\e6b609fe-ec72-4754-a496-7d225aa5fbc6` |
| OrigName | `string` | Original name of the file. | `example.txt` |
| OrigDirectory | `string` | Name of the Workspace directory from which the file was downloaded. | `example_dir` |
| OrigDirectoryId | `string` | ID of the Workspace directory from which the file was downloaded. |  |
| CreationTime | `DateTime` | Time (in UTC) when the file was downloaded. | `2021-10-19T09:58:43.978606Z` |

## UploadFiles

Encrypts and uploads files from a given directory and its subdirectories to the root directory of a Workspace package. Files will be encrypted before being uploaded to the server. Subdirectories will be created if they don't exist.

### Properties

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| BaseUrl | `string` | SendSafely base URL | `https://app.sendsafely.com` |
| ApiKey | `string` | SendSafely API key |  |
| ApiSecret | `string` | SendSafely API secret |  |
| PackageId | `string` | Workspace package ID |  |
| PackageKeyCode | `string` | Workspace package key code |  |
| Path | `string` | Path to the directory that contains the files to be uploaded. | `C:\\example_dir` |

### Returns

Returns a list of objects with the following parameters.

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| Name | `string` | Name of the uploaded file or the created directory. | `example.txt` |
| Id | `string` | Id of the uploaded file or the created directory. |  |
| IsFile | `bool` | True if the new entry is a file, False if it is a directory. | `True` |

## GetDirectories

Retrieves meta data about a directory and its subdirectories in a Workspace package.

### Properties

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| BaseUrl | `string` | SendSafely base URL | `https://app.sendsafely.com` |
| ApiKey | `string` | SendSafely API key |  |
| ApiSecret | `string` | SendSafely API secret |  |
| PackageId | `string` | Workspace package ID |  |
| RootDirectoryId | `string` | Directory ID |  |

### Returns

Returns a list of objects with the following parameters.

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| DirectoryId | `string` | Directory ID |  |
| DirectoryName | `string` | Directory name | `example_dir` |
| UserDirectory | `SendSafely.Directory` | User directory |  |
| Files | `List<FileResponse>` | Meta data about the files in the directory. |  |
| SubDirectories | `Collection<DirectoryResponse>` | Meta data about the subdirectories in the directory. |  |

## CreateDirectory

Creates a directory in a Workspace package.

### Properties

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| BaseUrl | `string` | SendSafely base URL | `https://app.sendsafely.com` |
| ApiKey | `string` | SendSafely API key |  |
| ApiSecret | `string` | SendSafely API secret |  |
| PackageId | `string` | Workspace package ID |  |
| ParentDirectoryId | `string` | Directory ID |  |
| DirectoryName | `string` | Name of the new directory | `new_dir` |

### Returns

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| DirectoryId | `string` | Directory ID |  |
| DirectoryName | `string` | Directory name | `example_dir` |
| UserDirectory | `SendSafely.Directory` | User directory |  |
| Files | `List<FileResponse>` | Meta data about the files in the directory. |  |
| SubDirectories | `Collection<DirectoryResponse>` | Meta data about the subdirectories in the directory. |  |

## GetFiles

Retrieves meta data about all files in a Workspace package.

### Properties

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| BaseUrl | `string` | SendSafely base URL | `https://app.sendsafely.com` |
| ApiKey | `string` | SendSafely API key |  |
| ApiSecret | `string` | SendSafely API secret |  |
| PackageId | `string` | Workspace package ID |  |

### Returns

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| FileName | `string` | File name | `example.txt` |
| FileId | `string` | File ID |  |
| FileSize | `string` | File size | `114` |
| CreatedByEmail | `string` | Email of the user that created the file. | `user@hiq.fi` |
| CreatedById | `string` |  ID of the user that created the file. |  |
| Uploaded | `DateTime` | Time when the file was uploaded to the Workspace. | `2021-10-22T07:12:46` |
| UploadedStr | `string` | Time (as a string) when the file was uploaded to the Workspace. | `Fri Oct 22 at 10:12 (EEST)` |
| DirectoryName | `string` | Name of the directory containing the file. | `example_dir` |
| DirectoryId | `string` | ID of the directory containing the file. |  |

## GetPackageInformation

Fetches the latest package meta data about a specific package given the unique package ID.

### Properties

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| BaseUrl | `string` | SendSafely base URL | `https://app.sendsafely.com` |
| ApiKey | `string` | SendSafely API key |  |
| ApiSecret | `string` | SendSafely API secret |  |
| PackageId | `string` | Package ID |  |

### Returns

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| ContactGroups | `List<ContactGroup>` | A list of contact groups |  |
| IsWorkspace | `bool` | The flag specifying the package is a Workspace. | `true` |
| PackageDescriptor | `string` | The package descriptor | `Example` |
| RootDirectoryId | `string` | The root directory of a Workspace package |  |
| Status | `PackageStatus` | The status of the package | `0` |
| State | `string` | The state of the pakage | `PACKAGE_STATE_ACTIVE_COMPLETE` |
| PackageOwner | `string` | The Package Owner of the package | `user@hiq.fi` |
| PackageTimestamp | `DateTime` | The timestamp of when the package was finalized. | `2021-10-12T09:31:55` |
| Life | `int` | The current package life. The package life determines for how long the package should be available to the recipients. It's measured in days. | 0 |
| Approvers | `List<string>` | A list of approvers that are currently attached to the package. |  |
| Files | `List<SendSafely.File>` | A list of files that are currently attached to the package. |  |
| Recipients | `List<SendSafely.Recipient>` | A list of recipients that are currently attached to the package. |  |
| NeedsApprover | `bool` | NeedsApprover will be true when a package needs to add at least one approver before the package can be finalized. If the package is finalized without the approver, an exception will be thrown. | `false` |
| ServerSecret | `string` | The server secret makes together with the keycode up the encryption key. The server secret is specific to a package and passed from the server. |  |
| KeyCode | `string` | The keycode for the package. This key should always be kept client side and never be sent to the server. The keycode makes up one part of the encryption key. |  |
| PackageCode | `string` | The package code for the given package. The package code is a part of the URL that must be sent to the recipients. |  |
| PackageId | `string` | The package ID for the given package. |  |
| AllowReplyAll | `bool` | Allow reply all, false if BCC recipients | `false` |
| PackageParentId | `string` | Package parent ID |  |

## GetPackageInformationFromLink

Fetches the latest package meta data about a specific package given the package link.

### Properties

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| BaseUrl | `string` | SendSafely base URL | `https://app.sendsafely.com` |
| ApiKey | `string` | SendSafely API key |  |
| ApiSecret | `string` | SendSafely API secret |  |
| PackageLink | `string` | Package Link |  |

### Returns

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| ContactGroups | `List<ContactGroup>` | A list of contact groups |  |
| IsWorkspace | `bool` | The flag specifying the package is a Workspace. | `true` |
| PackageDescriptor | `string` | The package descriptor | `Example` |
| RootDirectoryId | `string` | The root directory of a Workspace package |  |
| Status | `PackageStatus` | The status of the package | `0` |
| State | `string` | The state of the pakage | `PACKAGE_STATE_ACTIVE_COMPLETE` |
| PackageOwner | `string` | The Package Owner of the package | `user@hiq.fi` |
| PackageTimestamp | `DateTime` | The timestamp of when the package was finalized. | `2021-10-12T09:31:55` |
| Life | `int` | The current package life. The package life determines for how long the package should be available to the recipients. It's measured in days. | 0 |
| Approvers | `List<string>` | A list of approvers that are currently attached to the package. |  |
| Files | `List<SendSafely.File>` | A list of files that are currently attached to the package. |  |
| Recipients | `List<SendSafely.Recipient>` | A list of recipients that are currently attached to the package. |  |
| NeedsApprover | `bool` | NeedsApprover will be true when a package needs to add at least one approver before the package can be finalized. If the package is finalized without the approver, an exception will be thrown. | `false` |
| ServerSecret | `string` | The server secret makes together with the keycode up the encryption key. The server secret is specific to a package and passed from the server. |  |
| KeyCode | `string` | The keycode for the package. This key should always be kept client side and never be sent to the server. The keycode makes up one part of the encryption key. |  |
| PackageCode | `string` | The package code for the given package. The package code is a part of the URL that must be sent to the recipients. |  |
| PackageId | `string` | The package ID for the given package. |  |
| AllowReplyAll | `bool` | Allow reply all, false if BCC recipients | `false` |
| PackageParentId | `string` | Package parent ID |  |

# Building

Clone a copy of the repository

`git clone https://github.com/CommunityHiQ/Frends.Community.SendSafely.git`

Rebuild the project

`dotnet build`

Run tests

`dotnet test`

Create a NuGet package

`dotnet pack --configuration Release`

# Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repository on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

# Change Log

| Version | Changes |
| ------- | ------- |
| 0.0.5   | Development still going on |
| 1.0.0   | First version. Includes DownloadFiles, UploadFiles, GetDirectories, GetFiles, GetPackageInformation. |
| 1.1.0   | Added new methods: CreateDirectory and GetPackageInformationFromLink. |
