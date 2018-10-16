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

        public MyExampleIStorageAccountFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDictionary<string, CloudStorageAccount> LoadStorageAccounts()
        {
            CloudStorageAccount tmp;
            Dictionary<string,CloudStorageAccount> list = new Dictionary<string, CloudStorageAccount>();
            if(CloudStorageAccount.TryParse(_connectionString, out tmp) && tmp != null)
                list.Add("ImageStorage", tmp);
            return list;
        }
    }
}