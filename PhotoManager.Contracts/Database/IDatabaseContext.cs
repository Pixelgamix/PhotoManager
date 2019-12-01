using System;
using System.Threading.Tasks;

namespace PhotoManager.Contracts.Database
{
    public interface IDatabaseContext
    {
        Task ExecuteAsync(Func<IRepositoryContext, Task> unitOfWork);
    }
}