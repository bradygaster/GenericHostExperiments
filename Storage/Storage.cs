using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace GenericHostExperiments.Storage
{
    public static class StorageAccountExtensions
    {
        /// <summary>
        /// Builds the list of Azure Storage Accounts defined from configuration.
        /// </summary>
        public static IHostBuilder UseAzureStorage(this IHostBuilder builder)
        {
            return builder.ConfigureServices((services) => {
                services.AddSingleton<IStorageAccountFactory, ConfigurationFileStorageAccountFactory>();
                services.AddHostedService<AzureStorage>();
            });
        }

        /// <summary>
        /// Provides direct access to the underlying dictionary of storage accounts 
        /// so that they can be manually loaded during the host-building phase.
        /// </summary>
        public static IHostBuilder UseAzureStorage(this IHostBuilder builder, 
            Func<IDictionary<string, CloudStorageAccount>> factory)
        {
            return builder.ConfigureServices((services) => {
                services.AddSingleton<IStorageAccountFactory>(
                    new ManuallyLoadedStorageAccountFactory(factory));
                services.AddHostedService<AzureStorage>();
            });
        }

        /// <summary>
        /// Provides customization ability if you want to create your own
        /// IStorageAccountFactory implementation. 
        /// </summary>
        public static IHostBuilder UseAzureStorage(this IHostBuilder builder, 
            IStorageAccountFactory factory)
        {
            return builder.ConfigureServices((services) => {
                services.AddSingleton<IStorageAccountFactory>(factory);
                services.AddHostedService<AzureStorage>();
            });
        }
    }

    public class AzureStorage : IHostedService
    {
        private readonly IStorageAccountFactory _storageAccountFactory;
        private IDictionary<string, CloudStorageAccount> _storageAccounts;

        public AzureStorage(IStorageAccountFactory storageAccountFactory, ILogger<AzureStorage> logger)
        {
            _storageAccountFactory = storageAccountFactory;
            Logger = logger;
        }

        public ILogger Logger { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("AzureStorage started");

            _storageAccounts = _storageAccountFactory.LoadStorageAccounts();

            foreach (var key in _storageAccounts.Keys)
            {
                Logger.LogInformation(
                    string.Format($"Storage Account {key} created with Blob Endpoint {_storageAccounts[key].BlobStorageUri.PrimaryUri}.")
                );
            }

            return Task.FromResult(0);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("AzureStorage stopped");
            return Task.FromResult(0);
        }
    }
}