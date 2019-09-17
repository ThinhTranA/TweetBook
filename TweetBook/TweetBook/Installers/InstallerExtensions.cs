using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetBook.Installers
{
    public static class InstallerExtensions
    {
        //Notice: the "this" next to IServiceCollection parameter, "this" keyword allow this extension method in to be called in the services.
        //More details: https://stackoverflow.com/questions/846766/use-of-this-keyword-in-formal-parameters-for-static-methods-in-c-sharp
        public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            // x can be read as the current type
            //var classesImplementingInstaller = typeof(Startup).Assembly.ExportedTypes.Where(x =>
            //      typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();

            //select all installer, make an instance of them, and then cass them as the interface
            var installers = typeof(Startup).Assembly.ExportedTypes.Where(x =>
                typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                        .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }

    }
}
