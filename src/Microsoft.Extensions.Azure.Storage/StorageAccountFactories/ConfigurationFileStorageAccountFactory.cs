using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;

namespace Microsoft.Extensions.Azure.Storage
{
    public class ConfigurationFileStorageAccountFactory : IStorageAccountFactory
    {
        private readonly IConfiguration _configuration;

        private Dictionary<string, CloudStorageAccount> _storageAccounts;

        public ConfigurationFileStorageAccountFactory(IConfiguration configuration,
            ILogger<ConfigurationFileStorageAccountFactory> logger)
        {
            _configuration = configuration;
            Logger = logger;
        }

        public ILogger Logger { get; }

        public CloudStorageAccount GetAccount(string name)
        {
            return _storageAccounts[name];
        }

        public IDictionary<string, CloudStorageAccount> LoadStorageAccounts()
        {
            var section = _configuration.GetSection("Azure:Storage");
            var items = section.GetChildren();

            _storageAccounts = new Dictionary<string, CloudStorageAccount>();
            CloudStorageAccount tmp = null;

            foreach (var item in items)
            {
                if (CloudStorageAccount.TryParse(item.Value, out tmp) && tmp != null)
                    _storageAccounts.Add(item.Key, tmp);
                else
                {

                }
            }
            
            return _storageAccounts;
        }
    }
}