using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;

namespace GenericHostExperiments.AzureStorage
{
    public class ConfigurationFileStorageAccountFactory : IStorageAccountFactory
    {
        private readonly IConfiguration _configuration;

        public ConfigurationFileStorageAccountFactory(IConfiguration configuration,
            ILogger<ConfigurationFileStorageAccountFactory> logger)
        {
            _configuration = configuration;
            Logger = logger;
        }

        public ILogger Logger { get; }

        public IDictionary<string, CloudStorageAccount> LoadStorageAccounts()
        {
            var section = _configuration.GetSection("Azure:Storage");
            var items = section.GetChildren();

            Dictionary<string, CloudStorageAccount> storageAccounts = new Dictionary<string, CloudStorageAccount>();
            CloudStorageAccount tmp = null;

            foreach (var item in items)
            {
                if (CloudStorageAccount.TryParse(item.Value, out tmp) && tmp != null)
                    storageAccounts.Add(item.Key, tmp);
                else
                {

                }
            }
            
            return storageAccounts;
        }
    }
}