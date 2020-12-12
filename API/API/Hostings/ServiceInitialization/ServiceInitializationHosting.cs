using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using API.Helpers;
using API.Hostings.ServiceInitialization.Abstraction;
using API.Helpers.Extentions.ServiceProvicer;

namespace API.Hostings.ServiceInitialization
{
    public class ServiceInitializationHosting : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        //private readonly IProgramStatusService _programStatusService;
        public ServiceInitializationHosting(IServiceProvider serviceProvider/*, IProgramStatusService programStatusService*/)
        {
            _serviceProvider = serviceProvider;
            //_programStatusService = programStatusService;
        }

        /*public static void ClearDatabase(DbContext context)
        {
            string command = "";
            context.Model.GetEntityTypes().ForEach(x => command += String.Format("delete from \"{0}\";", x.GetAnnotation("Relational:TableName").Value.ToString()));
            if (command.Length > 0)
                Console.WriteLine("Database cleared {0}!", context.Database.ExecuteSqlRaw(command));
        }*/

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var services = _serviceProvider.GetAllServiceDescriptors().Where(x => typeof(IMyService).IsAssignableFrom(x.Key));
                // Initialization

                services.Where(x => typeof(IOnInitService).IsAssignableFrom(x.Key)).ForEach(x => {
                    var type = x.Value.ImplementationType;
                    var method = type.GetMethod("OnInit", BindingFlags.Static | BindingFlags.Public);
                    if (method != null)
                        method.Invoke(null, method.GetParameters().Select(r => scope.ServiceProvider.GetRequiredService(r.ParameterType)).ToArray());
                });

                // First Initialization

                /*if (_programStatusService.Status.IsLoadedFirst)
                {
                    ClearDatabase(scope.ServiceProvider.GetRequiredService<DbContext>());
                    services.Where(x => typeof(IOnFirstInitService).IsAssignableFrom(x.Key)).ForEach(x =>
                    {
                        var type = x.Value.ImplementationType;
                        var method = type.GetMethod("OnFirstInit", BindingFlags.Static | BindingFlags.Public);
                        if (method != null)
                            method.Invoke(null, method.GetParameters().Select(r => scope.ServiceProvider.GetRequiredService(r.ParameterType)).ToArray());
                    });
                    _programStatusService.Status.IsLoadedFirst = false;
                    _programStatusService.Status = _programStatusService.Status;
                }*/
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}