using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PhotoManager.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
            
            var additionalJsonConfigurationViaEnv = System.Environment.GetEnvironmentVariable("PHOTOMANAGER_CONFIG");
            if (additionalJsonConfigurationViaEnv != null)
            {
                var pathToAdditionalJson = additionalJsonConfigurationViaEnv.Split('=')[1];
                builder.ConfigureAppConfiguration((hostingContext, config) =>
                    config.AddJsonFile(pathToAdditionalJson, optional: true, reloadOnChange: true));
            }

            var additionalJsonConfigurationViaArgs = args.FirstOrDefault(x =>
                x.StartsWith("PHOTOMANAGER_CONFIG=", System.StringComparison.OrdinalIgnoreCase));
            if (additionalJsonConfigurationViaArgs != null)
            {
                var pathToAdditionalJson = additionalJsonConfigurationViaArgs.Split('=')[1];
                builder.ConfigureAppConfiguration((hostingContext, config) =>
                    config.AddJsonFile(pathToAdditionalJson, optional: true, reloadOnChange: true));
            }

            return builder;
        }
    }
}