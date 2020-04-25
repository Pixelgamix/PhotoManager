using PhotoManager.Contracts.Database;
using PhotoManager.Contracts.Entities;

namespace PhotoManager.Contracts.Logic
{
    public interface IPathCreator
    {
        string CreatePath(DirectorySettings settings, Photo photo);
    }
}