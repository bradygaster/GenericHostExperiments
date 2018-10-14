using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GenericHostExperiments.Storage;
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
                    })
                    .ConfigureLogging((hostBuilderContext, loggingBuilder) => {
                        loggingBuilder.AddConsole();
                        loggingBuilder.AddDebug();
                    })
                    // loads from config file
                    .UseAzureStorage() 
                    // load the storage accounts manually
                    /*
                    .UseAzureStorage(() => { 
                        CloudStorageAccount tmp;
                        Dictionary<string,CloudStorageAccount> list = new Dictionary<string, CloudStorageAccount>();
                        if(CloudStorageAccount.TryParse("DefaultEndpointsProtocol=https;AccountName=yourstorageaccount;AccountKey=YOUR_KEY;EndpointSuffix=core.windows.net", out tmp) && tmp != null)
                            list.Add("ImageStorage", tmp);
                        return list;
                    })
                    */
                    // load using your own IStorageAccountFactory implementation 
                    //.UseAzureStorage(new MyExampleIStorageAccountFactory("DefaultEndpointsProtocol=https;AccountName=yourstorageaccount;AccountKey=YOUR_KEY;EndpointSuffix=core.windows.net"))
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
