using System;
using System.IO;
using System.Threading.Tasks;
using Moq;
using PhotoManager.Contracts.Database;
using PhotoManager.Contracts.Entities;
using PhotoManager.Contracts.Logic;
using PhotoManager.DataAccess.Database;
using Xunit;

namespace PhotoManager.DataAccess.Tests
{
    public class PhotoRepositoryTester
    {
        private readonly PhotoRepository _photoRepository;
        private Mock<IFileNameCreator> _fileNameCreatorMock;
        private Mock<IPathCreator> _pathCreatorMock;
        private Mock<DatabaseSettings> _databaseSettingsMock;
        
        public PhotoRepositoryTester()
        {
            CreateMocks();
            _photoRepository = new PhotoRepository(_databaseSettingsMock.Object, _fileNameCreatorMock.Object, _pathCreatorMock.Object);
        }

        private void CreateMocks()
        {
            _fileNameCreatorMock = new Mock<IFileNameCreator>();
            _databaseSettingsMock = new Mock<DatabaseSettings>();

            _fileNameCreatorMock.Setup(m => m.CreateFileName(It.IsAny<FileNameSettings>(), It.IsAny<Photo>()))
                .Returns("01_12345_test.jpg");
        }

        [Fact]
        public Task SaveAsync_PhotoNull_Exception() =>
            Assert.ThrowsAsync<ArgumentNullException>(() => _photoRepository.SaveAsync(null));

        [Fact]
        public Task SaveAsync_PhotoStoragePathNull_Exception()
        {
            // prepare test.
            _databaseSettingsMock.SetupGet(m => m.StoragePath).Returns((string) null);
            
            // run test.
            return Assert.ThrowsAsync<ArgumentNullException>(() => _photoRepository.SaveAsync(new Photo()));
        }
        
        [Fact]
        public Task SaveAsync_PhotoContentNull_Exception()
        {
            // prepare test.
            _databaseSettingsMock.SetupGet(m => m.StoragePath).Returns(".");
            
            // run test.
            return Assert.ThrowsAsync<ArgumentNullException>(() => _photoRepository.SaveAsync(new Photo{FileName = "Test.jpg"}));
        }
        
        [Fact]
        public void SaveAsync()
        {
            // prepare test.
            _databaseSettingsMock.SetupGet(m => m.StoragePath).Returns(".");
            
            var testImageBytes = File.ReadAllBytes("./Testdata/Test.jpg");
            
            var photo = new Photo
            {
                FileName = "test.jpg",
                Content = testImageBytes
            };
            
            // run test.
            _photoRepository.SaveAsync(photo);

            // compare result.
            const string path = "./01_12345_test.jpg";
            
            Assert.True(File.Exists(path));
            
            var resultFile = File.ReadAllBytes(path);
            
            Assert.Equal(testImageBytes, resultFile);

        }
    }
}