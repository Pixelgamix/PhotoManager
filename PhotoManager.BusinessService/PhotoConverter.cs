using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using PhotoManager.Contracts.Entities;
using PhotoManager.Contracts.Logic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace PhotoManager.BusinessService
{
    public class PhotoConverter : IPhotoConverter
    {
        #region Public methods

        /// <summary>
        /// Converts a stream, filename and creation date into a Photo-Entity.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="fileName">Filename.</param>
        /// <param name="creationDate">Creation date.</param>
        /// <returns><see cref="Photo"> Photo.</see></returns>
        public async Task<Photo> ConvertToPhoto(Stream stream, string fileName, DateTime creationDate)
        {
            if(stream == null) throw new ArgumentNullException(nameof(stream));
            if(fileName == null) throw new ArgumentNullException(nameof(fileName));
            
            // read stream.
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, (int)stream.Length);
            
            CheckFileType(bytes);
            
            // put the byte-array into a MemoryStream to read the metadata.
            await using var memStream = new MemoryStream(bytes);
            
            return new Photo()
            {
                Content = bytes,
                CreationTimestamp = GetCreationDate(memStream, creationDate),
                FileName = fileName
            };
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method defines the creation date by the photo metadata.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="creationDate">creation date will be used, if there is no metadata at the photo.</param>
        /// <returns>Creation date.</returns>
        private static DateTime GetCreationDate(Stream stream, DateTime creationDate)
        {
            var fileInfo = Image.Identify(stream);
            
            ExifValue creationDateValue = null;
            fileInfo.Metadata?.ExifProfile?.TryGetValue(ExifTag.DateTimeOriginal, out creationDateValue);
            if (creationDateValue != null)
                return DateTime.ParseExact(creationDateValue.ToString(), "yyyy:MM:dd HH:mm:ss", 
                    CultureInfo.CurrentCulture);

            return creationDate;
        }

        /// <summary>
        /// Method checks for a photo-filetype.
        /// </summary>
        /// <param name="bytes">byte-array.</param>
        /// <exception cref="WrongFileTypeException"></exception>
        private static void CheckFileType(byte[] bytes)
        {
            var imgFormat = Image.DetectFormat(bytes);
            
            // null = no image!
            if (imgFormat == null)
                throw new WrongFileTypeException("wrong file type");
        }

        #endregion
    }
}
