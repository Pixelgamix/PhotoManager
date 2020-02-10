namespace PhotoManager.Contracts.Database
{
    public interface IRepositoryContext
    {
        IPhotoRepository PhotoRepository { get; }
    }
}