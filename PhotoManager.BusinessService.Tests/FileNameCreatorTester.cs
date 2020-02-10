using System;
using System.Collections.Generic;
using PhotoManager.Contracts.Database;
using PhotoManager.Contracts.Entities;
using PhotoManager.Contracts.Logic;
using Xunit;

namespace PhotoManager.BusinessService.Tests
{
    public class FileNameCreatorTester
    {
        private Photo _testPhoto = new Photo{FileName = "Test.jpg"};
        private FileNameSettings _testSettings = new FileNameSettings{SeparateYearMonthDay = true, Separator = char.Parse("_")};
        
        private readonly FileNameCreator _fileNameCreator;

        public FileNameCreatorTester()
        {
            _fileNameCreator = new FileNameCreator();
        }

        [Fact]
        public void CreateFileName_FilenameSettingsNull_Exception() =>
            Assert.Throws<ArgumentNullException>(() => _fileNameCreator.CreateFileName(
                null, 
                new Photo{FileName = "Test.jpg"}));
        
        [Fact]
        public void CreateFileName_PhotoNull_Exception() =>
            Assert.Throws<ArgumentNullException>(() => _fileNameCreator.CreateFileName(
                new FileNameSettings{SeparateYearMonthDay = true, Separator = char.Parse("_")},
                null));
        
        [Fact]
        public void CreateFileName_PhotoFileNameNull_Exception() =>
            Assert.Throws<ArgumentNullException>(() => _fileNameCreator.CreateFileName(
                new FileNameSettings{SeparateYearMonthDay = true, Separator = char.Parse("_")},
                new Photo()));

        [Fact]
        public void CreateFileName_PhotoFileNameLengthToShort_Exception()
        {
            var ex = Assert.Throws<WrongFileNameException>(() => _fileNameCreator.CreateFileName(
                new FileNameSettings{SeparateYearMonthDay = true, Separator = char.Parse("_")},
                new Photo{FileName = "a.jp"}));
            Assert.Equal("Filename must have at least 5 characters.", ex.Message);
        }
         
        [Fact]
        public void CreateFileName_PhotoFileNameTypeToShort_Exception()
        {
            var ex = Assert.Throws<WrongFileNameException>(() => _fileNameCreator.CreateFileName(
                new FileNameSettings{SeparateYearMonthDay = true, Separator = char.Parse("_")},
                new Photo{FileName = "ab.jp"}));
            Assert.Equal("Filetype must have min 3 characters.", ex.Message);
        }
        
        [Fact]
        public void CreateFileName_AllPartsAreSeparated()
        {
            // prepare test.
            var photo = new Photo
            {
                FileName = "test.jpg",
                CreationTimestamp = new DateTime(2019,12,10,14,22,5)
            };
            var settings = new FileNameSettings
            {
                Separator = char.Parse("_"),
                SeparateYearMonthDay = true,
                FileNameOrder = new List<string>
                {
                    FileNameFormat.Year, 
                    FileNameFormat.Month, 
                    FileNameFormat.Day, 
                    FileNameFormat.Time, 
                    FileNameFormat.OriginFileName,
                    "ignored tag!"
                }
            };
            
            // run test.
            var fileName = _fileNameCreator.CreateFileName(settings, photo);
            
            // compare result.
            Assert.Equal("2019_12_10_142205_test.jpg", fileName);
        }
        
        [Fact]
        public void CreateFileName_CreationDateIsCombinedWithoutTime()
        {
            // prepare test.
            var photo = new Photo
            {
                FileName = "test.jpg",
                CreationTimestamp = new DateTime(2019,12,10,14,22,5)
            };
            var settings = new FileNameSettings
            {
                Separator = char.Parse("_"),
                SeparateYearMonthDay = false,
                SeparateTimeFromDate = true,
                FileNameOrder = new List<string>
                {
                    FileNameFormat.Year, 
                    FileNameFormat.Month, 
                    FileNameFormat.Day,  
                    FileNameFormat.OriginFileName,
                    "ignored tag!"
                }
            };
            
            // run test.
            var fileName = _fileNameCreator.CreateFileName(settings, photo);
            
            // compare result.
            Assert.Equal("20191210_test.jpg", fileName);
        }
        
        [Fact]
        public void CreateFileName_CreationDateIsCombinedWithTime()
        {
            // prepare test.
            var photo = new Photo
            {
                FileName = "test.jpg",
                CreationTimestamp = new DateTime(2019,12,10,14,22,5)
            };
            var settings = new FileNameSettings
            {
                Separator = char.Parse("_"),
                SeparateYearMonthDay = false,
                SeparateTimeFromDate = false,
                FileNameOrder = new List<string>
                {
                    FileNameFormat.Year, 
                    FileNameFormat.Month, 
                    FileNameFormat.Day,
                    FileNameFormat.Time,
                    FileNameFormat.OriginFileName,
                    "ignored tag!"
                }
            };
            
            // run test.
            var fileName = _fileNameCreator.CreateFileName(settings, photo);
            
            // compare result.
            Assert.Equal("20191210142205_test.jpg", fileName);
        }
        
        [Fact]
        public void CreateFileName_CreationDateIsCombinedWithTimeSeparated()
        {
            // prepare test.
            var photo = new Photo
            {
                FileName = "test.jpg",
                CreationTimestamp = new DateTime(2019,12,10,14,22,5)
            };
            var settings = new FileNameSettings
            {
                Separator = char.Parse("_"),
                SeparateYearMonthDay = false,
                SeparateTimeFromDate = true,
                FileNameOrder = new List<string>
                {
                    FileNameFormat.Year, 
                    FileNameFormat.Month, 
                    FileNameFormat.Day,
                    FileNameFormat.Time,
                    FileNameFormat.OriginFileName,
                    "ignored tag!"
                }
            };
            
            // run test.
            var fileName = _fileNameCreator.CreateFileName(settings, photo);
            
            // compare result.
            Assert.Equal("20191210_142205_test.jpg", fileName);
        }
        
        [Fact]
        public void CreateFileName_OnlyOriginFileName()
        {
            // prepare test.
            var photo = new Photo
            {
                FileName = "test.jpg",
                CreationTimestamp = new DateTime(2019,12,10,14,22,5)
            };
            var settings = new FileNameSettings
            {
                Separator = char.Parse("_"),
                SeparateYearMonthDay = false,
                SeparateTimeFromDate = false,
                FileNameOrder = new List<string>
                {
                    FileNameFormat.OriginFileName,
                    "ignored tag!"
                }
            };
            
            // run test.
            var fileName = _fileNameCreator.CreateFileName(settings, photo);
            
            // compare result.
            Assert.Equal("test.jpg", fileName);
        }
        
        [Fact]
        public void CreateFileName_CreationDateOnlyDayAndTime()
        {
            // prepare test.
            var photo = new Photo
            {
                FileName = "test.jpg",
                CreationTimestamp = new DateTime(2019,12,10,14,22,5)
            };
            var settings = new FileNameSettings
            {
                Separator = char.Parse("_"),
                SeparateYearMonthDay = false,
                SeparateTimeFromDate = true,
                FileNameOrder = new List<string>
                {
                    FileNameFormat.Day,
                    FileNameFormat.Time,
                    FileNameFormat.OriginFileName,
                    "ignored tag!"
                }
            };
            
            // run test.
            var fileName = _fileNameCreator.CreateFileName(settings, photo);
            
            // compare result.
            Assert.Equal("10_142205_test.jpg", fileName);
        }
    }
}