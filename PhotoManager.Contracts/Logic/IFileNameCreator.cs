using PhotoManager.Contracts.Database;
using PhotoManager.Contracts.Entities;

namespace PhotoManager.Contracts.Logic
{
    public interface IFileNameCreator
    {
        string CreateFileName(FileNameSettings fileNameSettings, Photo photo);
    }
}