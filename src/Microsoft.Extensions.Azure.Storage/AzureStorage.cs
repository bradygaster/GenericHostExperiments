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

namespace Microsoft.Extensions.Azure.Storage
{
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