using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using PhotoManager.Contracts.Entities;
using PhotoManager.Contracts.Logic;


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
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(stream);
            var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            var dateTime = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);
            if (dateTime != null)
                return DateTime.ParseExact(dateTime.ToString(), "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture);

            return creationDate;
        }

        /// <summary>
        /// Method checks for a photo-filetype.
        /// </summary>
        /// <param name="bytes">byte-array.</param>
        /// <exception cref="WrongFileTypeException"></exception>
        private static void CheckFileType(byte[] bytes)
        {
            
            //var imgFormat = Image.DetectFormat(bytes);
            
            //// null = no image!
            //if (imgFormat == null)
            //    throw new WrongFileTypeException("wrong file type");
        }

        #endregion
    }
}
