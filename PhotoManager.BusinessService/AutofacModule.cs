using Autofac;
using PhotoManager.Contracts.Logic;

namespace PhotoManager.BusinessService
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileNameCreator>()
                .As<IFileNameCreator>();
        }
    }
}