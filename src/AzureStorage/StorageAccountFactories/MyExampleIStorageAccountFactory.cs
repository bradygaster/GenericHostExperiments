using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;

namespace GenericHostExperiments.AzureStorage
{
    public class MyExampleIStorageAccountFactory : IStorageAccountFactory
    {
        string _connectionString;
        private CloudStorageAccount _storageAccount;

        public MyExampleIStorageAccountFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public CloudStorageAccount GetAccount(string name)
        {
            return _storageAccount;
        }

        public IDictionary<string, CloudStorageAccount> LoadStorageAccounts()
        {
            if(CloudStorageAccount.TryParse(_connectionString, out _storageAccount) 
                && _storageAccount != null)
            {
                Dictionary<string,CloudStorageAccount> list = new Dictionary<string, CloudStorageAccount>();
                list.Add("ImageStorage", _storageAccount);
                return list;
            }

            return null;
        }
    }
}