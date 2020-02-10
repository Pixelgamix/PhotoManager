using System.Threading.Tasks;
using PhotoManager.Contracts.Entities;

namespace PhotoManager.Contracts.Database
{
    public interface IPhotoRepository
    {
        Task SaveAsync(Photo photo);
    }
}