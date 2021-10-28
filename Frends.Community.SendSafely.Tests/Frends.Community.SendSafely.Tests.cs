using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Frends.Community.SendSafely.Tests
{
    [TestFixture]
    class TestClass
    {
        public Parameters parameters = new Parameters
        {
            BaseUrl = "",
            ApiKey = "",
            ApiSecret = ""
        };

        [Ignore("This is used to test with an actual SendSafely API.")]
        [Test]
        public void DownloadFiles()
        {
            var input = new DownloadFilesInput
            {
                PackageId = "",
                DirectoryId = "",
                PackageKeyCode = ""
            };

            IEnumerable<string> expectedFileNames = new[] { "test.txt" };
            IEnumerable<DownloadFilesResult> result = SendSafelyTasks.DownloadFiles(parameters, input, new System.Threading.CancellationToken());
            IEnumerable<string> actualFileNames = result.Select(f => f.OrigName);    

            Assert.That(actualFileNames, Is.EquivalentTo(expectedFileNames));
        }

        [Ignore("This is used to test with an actual SendSafely API.")]
        [Test]
        public void DownloadFilesWithDate()
        {
            var input = new DownloadFilesInput
            {
                PackageId = "",
                DirectoryId = "",
                PackageKeyCode = "",
                Date = "2021-10-25T00:00:00"
            };

            IEnumerable<string> expectedFileNames = new[] { "test.txt" };
            IEnumerable<DownloadFilesResult> result = SendSafelyTasks.DownloadFiles(parameters, input, new System.Threading.CancellationToken());
            IEnumerable<string> actualFileNames = result.Select(f => f.OrigName);

            Assert.That(actualFileNames, Is.EquivalentTo(expectedFileNames));
        }

        [Ignore("This is used to test with an actual SendSafely API.")]
        [Test]
        public void UploadFiles()
        {
            var input = new UploadFilesInput
            {
                PackageId = "",
                PackageKeyCode = "",
                Path = ""
            };

            IEnumerable<string> expectedFileNames = new[] { "test.txt" };
            IEnumerable<UploadFilesResult> result = SendSafelyTasks.UploadFiles(parameters, input, new System.Threading.CancellationToken());
            IEnumerable<string> actualFileNames = result.Select(f => f.Name);

            Assert.That(actualFileNames, Is.EquivalentTo(expectedFileNames));
        }

        [Ignore("This is used to test with an actual SendSafely API.")]
        [Test]
        public void GetDirectories()
        {
            var input = new GetDirectoriesInput
            {
                PackageId = "",
                RootDirectoryId = "",
            };

            IEnumerable<string> expectedDirNames = new[] { "/" };
            IEnumerable<GetDirectoriesResult> result = SendSafelyTasks.GetDirectories(parameters, input, new System.Threading.CancellationToken());
            IEnumerable<string> actualDirNames = result.Select(d => d.DirectoryName);

            Assert.That(actualDirNames, Is.EquivalentTo(expectedDirNames));
        }

        [Ignore("This is used to test with an actual SendSafely API.")]
        [Test]
        public void GetFiles()
        {
            var input = new GetFilesInput
            {
                PackageId = ""
            };

            IEnumerable<string> expectedFileNames = new[] { "test.txt" };
            IEnumerable<GetFilesResult> result = SendSafelyTasks.GetFiles(parameters, input, new System.Threading.CancellationToken());
            IEnumerable<string> actualFileNames = result.Select(f => f.FileName);

            Assert.That(actualFileNames, Is.EquivalentTo(expectedFileNames));
        }

        [Ignore("This is used to test with an actual SendSafely API.")]
        [Test]
        public void GetPackageInformation()
        {
            var input = new GetPackageInformationInput
            {
                PackageId = ""
            };

            string expectedRootDirId = "";
            GetPackageInformationResult pkgInfo = SendSafelyTasks.GetPackageInformation(parameters, input);
            string actualRootDirId = pkgInfo.RootDirectoryId;

            Assert.That(actualRootDirId, Is.EqualTo(expectedRootDirId));
        }
    }
}
