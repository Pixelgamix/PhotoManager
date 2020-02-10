using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace PhotoManager.BusinessService.Tests
{
    public class PhotoConverterTester
    {
        private readonly MemoryStream _jpegStream = new MemoryStream(new byte[]{255,216,255,217});
        private readonly DateTime _creationDate = new DateTime(2019, 12, 5, 13,59,45);
        private const string FileName = "Test.jpg";

        private readonly PhotoConverter _photoConverter;
        
        public PhotoConverterTester()
        {
            _photoConverter = new PhotoConverter();
        }
        
        [Fact]
        public Task ConvertToPhoto_streamIsNull_Exception() => 
            Assert.ThrowsAsync<ArgumentNullException>(() => 
                _photoConverter.ConvertToPhoto(null, FileName, _creationDate));
        
        [Fact]
        public Task ConvertToPhoto_fileNameIsNull_Exception() => 
            Assert.ThrowsAsync<ArgumentNullException>(() => 
                _photoConverter.ConvertToPhoto(_jpegStream, null, _creationDate));
        
        
        [Fact]
        public void ConvertToPhoto_streamIsJpgWithoutMetadata()
        {
            var testImageBytes = File.ReadAllBytes("./Testdata/Test.jpg");
            var stream = new MemoryStream(testImageBytes);
            
            var photoConverter = new PhotoConverter();
            var result = photoConverter.ConvertToPhoto(stream, FileName, _creationDate);
            
            Assert.Equal(_creationDate, result.Result.CreationTimestamp);
        }
        
        [Fact]
        public void ConvertToPhoto_streamIsJpgWithMetadata()
        {
            var testImageBytes = File.ReadAllBytes("./Testdata/20081209_DSC00228.JPG");
            var stream = new MemoryStream(testImageBytes);
            
            var photoConverter = new PhotoConverter();
            var result = photoConverter.ConvertToPhoto(stream, FileName, _creationDate);
            
            Assert.Equal(new DateTime(2007,2,16,14,24,40), result.Result.CreationTimestamp);
        }
    }
}
