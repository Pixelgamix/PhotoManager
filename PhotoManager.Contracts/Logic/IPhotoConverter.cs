using System;
using System.IO;
using System.Threading.Tasks;
using PhotoManager.Contracts.Entities;

namespace PhotoManager.Contracts.Logic
{
    public interface IPhotoConverter
    {
        Task<Photo> ConvertToPhoto(Stream stream, string fileName, DateTime creationDate);
    }
}