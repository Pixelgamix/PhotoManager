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
        private readonly IPathCreator _pathCreator;

        #endregion

        #region Constructors

        public PhotoRepository(DatabaseSettings databaseSettings, IFileNameCreator fileNameCreator, IPathCreator pathCreator)
        {
            _databaseSettings = databaseSettings ?? throw new ArgumentNullException(nameof(databaseSettings));
            _fileNameCreator = fileNameCreator ?? throw new ArgumentNullException(nameof(fileNameCreator));
            _pathCreator = pathCreator ?? throw new ArgumentNullException(nameof(pathCreator));
        }

        #endregion

        #region Public methods

        public Task SaveAsync(Photo photo)
        {
            if (photo == null) throw new ArgumentNullException(nameof(photo));

            var path = _pathCreator.CreatePath(_databaseSettings.DirectoryFormat, photo);
            var fileName = _fileNameCreator.CreateFileName(_databaseSettings.FileNameSettings, photo);
            return WriteContent(Path.Combine(_databaseSettings.StoragePath, path), fileName, photo.Content);
        }

        #endregion

        #region Private methods

        private Task WriteContent(string storagePath, string fileName, byte[] content)
        {
            if (storagePath == null) throw new ArgumentNullException(nameof(storagePath));
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (content == null) throw new ArgumentNullException(nameof(content));

            if (!Directory.Exists(storagePath))
                Directory.CreateDirectory(storagePath);

            var path = Path.Combine(storagePath, fileName);
            
            return File.WriteAllBytesAsync(path, content);
        }

        #endregion
    }
}
