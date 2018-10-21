using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Azure.Storage;
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
                    .UseAzureStorage() // loads from config file (see README.md for other methods)
                    .ConfigureServices((services) => {
                        services.AddLogging();
                        services.AddHostedService<DemoQueueListenerService>();
                        services.AddHostedService<DemoQueueFeedService>();
                    })
                    .ConfigureLogging((hostBuilderContext, loggingBuilder) => {
                        loggingBuilder.AddConsole();
                        loggingBuilder.AddDebug();
                    })
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
