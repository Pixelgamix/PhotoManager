using System;
using System.Threading.Tasks;
using PhotoManager.Contracts.Database;
using PhotoManager.Contracts.Entities;

namespace PhotoManager.DataAccess.Database
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DatabaseSettings _databaseSettings;

        public PhotoRepository(DatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings ?? throw new ArgumentNullException(nameof(databaseSettings));
        }
        
        public Task SaveAsync(Photo photo)
        {
            throw new System.NotImplementedException();
        }
    }
}
