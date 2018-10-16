using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GenericHostExperiments.AzureStorage;
using Microsoft.WindowsAzure.Storage;
using System.Collections.Generic;

namespace GenericHostExperiments
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var host = new HostBuilder()
                    .ConfigureHostConfiguration(configHost =>
                    {
                        configHost.SetBasePath(Directory.GetCurrentDirectory());
                        configHost.AddJsonFile("hostsettings.json", optional: true);
                    })
                    .ConfigureAppConfiguration((hostBuilderContext, configApp) =>
                    {
                        configApp.SetBasePath(Directory.GetCurrentDirectory());
                        configApp.AddJsonFile("appsettings.json", optional: true);
                    })
                    .ConfigureServices((services) => {
                        services.AddLogging();
                        services.AddSingleton<IHostedService,DemoQueueFeedService>();
                        services.AddSingleton<IHostedService,DemoQueueListenerService>();
                    })
                    .ConfigureLogging((hostBuilderContext, loggingBuilder) => {
                        loggingBuilder.AddConsole();
                        loggingBuilder.AddDebug();
                    })
                    .UseAzureStorage() // loads from config file (see README.md for other methods)
                    .Build();

                await host.RunAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
