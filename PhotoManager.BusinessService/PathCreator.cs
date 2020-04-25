using System;
using System.Globalization;
using System.IO;
using PhotoManager.Contracts.Database;
using PhotoManager.Contracts.Entities;
using PhotoManager.Contracts.Logic;

namespace PhotoManager.BusinessService
{
    public class PathCreator : IPathCreator
    {
        public string CreatePath(DirectorySettings settings, Photo photo) => this.GetDir(settings, photo);

        private string GetDir(DirectorySettings settings, Photo photo)
        {
            var currentFolder = ConvertPath(settings.Directory, photo); 
            return settings.SubDirectory == null ? currentFolder : Path.Combine(currentFolder, this.GetDir(settings.SubDirectory, photo));
        }

        private static string ConvertPath(string directoryFormat, Photo photo)
        {
            return directoryFormat switch
            {
                FileNameFormat.Year => GetNextFileNamePartNumber(photo.CreationTimestamp.Year, 4),
                FileNameFormat.Month => GetMonthName(photo.CreationTimestamp),
                FileNameFormat.Day => GetNextFileNamePartNumber(photo.CreationTimestamp.Day, 2),
                _ => string.Empty
            };
        }

        private static string GetMonthName(DateTime dateTime)
            => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        
        private static string GetNextFileNamePartNumber(int number, int digits) 
            => number.ToString().PadLeft(digits, char.Parse("0"));
        
    }
}