using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using PhotoManager.Contracts.Entities;
using PhotoManager.Contracts.Logic;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace PhotoManager.BusinessService
{
    public class PhotoConverter : IPhotoConverter
    {
        public async Task<Photo> ConvertToPhoto(Stream stream, string fileName, DateTime creationDate)
        {
            // read stream.
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, (int)stream.Length);
            
            var imgFormat = SixLabors.ImageSharp.Image.DetectFormat(bytes);
            if (imgFormat == null)
            {
                //kein bild
                throw new Exception("kein Gültiges Format");
            }
            
            using var memStream = new MemoryStream(bytes);

            var fileInfo = SixLabors.ImageSharp.Image.Identify(memStream);
            ExifValue creationDateValue = null;
            fileInfo.Metadata?.ExifProfile?.TryGetValue(ExifTag.DateTimeOriginal, out creationDateValue);

            
            if (creationDateValue != null)
            {
                creationDate = DateTime.ParseExact(creationDateValue.ToString(), "yyyy:MM:dd hh:mm:ss", CultureInfo.CurrentCulture);    
            }
            
            
            return new Photo()
            {
                Content = bytes,
                CreationTimestamp = creationDate,
                FileName = fileName
            };
        }
    }
}