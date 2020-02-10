using System;
using System.Threading.Tasks;
using PhotoManager.Contracts.Database;

namespace PhotoManager.DataAccess.Database
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly IRepositoryContext _repositoryContext;

        public DatabaseContext(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext ?? throw new ArgumentNullException(nameof(repositoryContext));
        }
        
        public Task ExecuteAsync(Func<IRepositoryContext, Task> unitOfWork)
        {
            return unitOfWork(_repositoryContext);
        }
    }
}