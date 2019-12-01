using Autofac;
using PhotoManager.Contracts.Database;
using PhotoManager.DataAccess.Database;

namespace PhotoManager.DataAccess
{
    public sealed class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DatabaseContext>()
                .As<IDatabaseContext>();

            builder.RegisterType<RepositoryContext>()
                .As<IRepositoryContext>();

            builder.RegisterType<PhotoRepository>()
                .As<IPhotoRepository>();
        }
    }
}