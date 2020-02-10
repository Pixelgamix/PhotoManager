using System;
using PhotoManager.Contracts.Database;

namespace PhotoManager.DataAccess.Database
{
    public class RepositoryContext : IRepositoryContext
    {
        public RepositoryContext(PhotoRepository photoRepository)
        {
            PhotoRepository = photoRepository ?? throw new ArgumentNullException(nameof(photoRepository));
        }
        
        public IPhotoRepository PhotoRepository { get; }
    }
}