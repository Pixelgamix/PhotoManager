using System;
using System.IO;
using System.Threading.Tasks;
using PhotoManager.Contracts.Database;
using PhotoManager.Contracts.Entities;
using PhotoManager.Contracts.Logic;

namespace PhotoManager.DataAccess.Database
{
    public class PhotoRepository : IPhotoRepository
    {

        #region Private member

        private readonly DatabaseSettings _databaseSettings;
        private readonly IFileNameCreator _fileNameCreator;

        #endregion

        #region Constructors

        public PhotoRepository(DatabaseSettings databaseSettings, IFileNameCreator fileNameCreator)
        {
            _databaseSettings = databaseSettings ?? throw new ArgumentNullException(nameof(databaseSettings));
            _fileNameCreator = fileNameCreator ?? throw new ArgumentNullException(nameof(fileNameCreator));
        }

        #endregion

        #region Public methods

        public Task SaveAsync(Photo photo)
        {
            if (photo == null) throw new ArgumentNullException(nameof(photo));

            var fileName = _fileNameCreator.CreateFileName(_databaseSettings.FileNameSettings, photo);
            return WriteContent(_databaseSettings.StoragePath, fileName, photo.Content);
        }

        #endregion

        #region Private methods

        private Task WriteContent(string storagePath, string fileName, byte[] content)
        {
            if (storagePath == null) throw new ArgumentNullException(nameof(storagePath));
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (content == null) throw new ArgumentNullException(nameof(content));

            var path = Path.Combine(_databaseSettings.StoragePath, fileName);
            
            return File.WriteAllBytesAsync(path, content);
        }

        #endregion
    }
}
