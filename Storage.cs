using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace GenericHostExperiments
{
    public static class StorageAccountExtensions
    {
        public static IHostBuilder UseAzureStorage(this IHostBuilder builder)
        {
            return builder.ConfigureServices((services) => {
                services.AddHostedService<AzureStorage>();
            });
        }
    }

    public class AzureStorage : IHostedService
    {
        private IConfiguration _configuration;
        private Dictionary<string, CloudStorageAccount> _storageAccounts;

        public AzureStorage(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("AzureStorage started");

            var section = _configuration.GetSection("Azure:Storage");
            var items = section.GetChildren();

            _storageAccounts = new Dictionary<string,CloudStorageAccount>();

            foreach (var item in items)
            {
                Console.WriteLine($"Trying Storage Account {item.Key}.");

                CloudStorageAccount tmp = null;

                if(CloudStorageAccount.TryParse(item.Value, out tmp) && tmp != null)
                {
                    _storageAccounts.Add(item.Key, tmp);
                    Console.WriteLine($"Storage Account {item.Key} added successfully.");
                }
            }

            return Task.FromResult(0);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("AzureStorage stopped");
            return Task.FromResult(0);
        }
    }
}