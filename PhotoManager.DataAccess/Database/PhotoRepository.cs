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
        private readonly DatabaseSettings _databaseSettings;
        private readonly IFileNameCreator _fileNameCreator;

        public PhotoRepository(DatabaseSettings databaseSettings, IFileNameCreator fileNameCreator)
        {
            _databaseSettings = databaseSettings ?? throw new ArgumentNullException(nameof(databaseSettings));
            _fileNameCreator = fileNameCreator ?? throw new ArgumentNullException(nameof(fileNameCreator));
        }
        
        public Task SaveAsync(Photo photo)
        {
            var fileName = _fileNameCreator.CreateFileName(_databaseSettings.FileNameSettings, photo);
            return File.WriteAllBytesAsync(Path.Combine(_databaseSettings.StoragePath, fileName), photo.Content);
        }
    }
}
