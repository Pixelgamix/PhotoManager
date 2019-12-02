using System;
using System.Collections.Generic;
using System.Linq;
using PhotoManager.Contracts.Database;
using PhotoManager.Contracts.Entities;
using PhotoManager.Contracts.Logic;

namespace PhotoManager.BusinessService
{
    public class FileNameCreator : IFileNameCreator
    {
        #region Private member

        private static readonly string[] CreationFormatWithTime = {
            FileNameFormat.Year, 
            FileNameFormat.Month, 
            FileNameFormat.Day, 
            FileNameFormat.Time
        };
        
        private static readonly string[] CreationFormatWithoutTime =
        {
            FileNameFormat.Year, 
            FileNameFormat.Month, 
            FileNameFormat.Day
        };
        
        #endregion

        #region Public methods

        public string CreateFileName(FileNameSettings fileNameSettings, Photo photo)
        {
            var creationFormats = GetCreationFormat(fileNameSettings);

            var fileName = string.Empty;
            foreach (var fileNamePart in fileNameSettings.FileNameOrder)
            {
                var separator = GetSeparator(fileNameSettings.Separator.ToString(), creationFormats, fileNamePart, 
                    fileNameSettings.FileNameOrder, true);

                fileName = fileNamePart switch
                {
                    FileNameFormat.Year => string.Concat(fileName, separator, photo.CreationTimestamp.Year),
                    FileNameFormat.Month => string.Concat(fileName, separator, photo.CreationTimestamp.Month),
                    FileNameFormat.Day => string.Concat(fileName, separator, photo.CreationTimestamp.Day),
                    FileNameFormat.Time => string.Concat(fileName, separator, photo.CreationTimestamp.ToString("hhmmss")),
                    FileNameFormat.OriginFileName => string.Concat(fileName, separator, photo.FileName),
                    FileNameFormat.UploadDate => string.Concat(fileName, separator, DateTime.Now.ToString("yyyyMMdd")),
                    _ => fileName
                };
            }

            return fileName;
        }

        #endregion

        #region Private methods

        private static string GetSeparator(string orgSeparator, string[] creationFormats, string fileNamePart, 
            IList<string> orderedFileNameParts, bool firstElementEmpty)
        {
            // return separator if it's the first element.
            if (orderedFileNameParts.IndexOf(fileNamePart) == 0)
                return firstElementEmpty ? string.Empty : orgSeparator;

            // if creationFormats is not defined, all element will be separated.
            if (creationFormats == null)
                return orgSeparator;

            // is the current and the previous part both creationFormats, no separator needed.
            var prevFileNamePart = orderedFileNameParts[orderedFileNameParts.IndexOf(fileNamePart) - 1];
            if (creationFormats.Contains(fileNamePart) && creationFormats.Contains(prevFileNamePart))
                return string.Empty;
            
            return orgSeparator;
        }
        
        private static string[] GetCreationFormat(FileNameSettings fileNameSettings)
        {
            if (fileNameSettings.SeparateYearMonthDay) return null;
            
            return !fileNameSettings.SeparateTimeFromDate ? CreationFormatWithTime : CreationFormatWithoutTime;
        }

        #endregion
    }
}
