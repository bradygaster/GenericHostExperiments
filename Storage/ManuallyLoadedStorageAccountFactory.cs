using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;

namespace GenericHostExperiments.Storage
{
    public class ManuallyLoadedStorageAccountFactory : IStorageAccountFactory
    {
        IDictionary<string,CloudStorageAccount> _storageAccounts;

        public ManuallyLoadedStorageAccountFactory(Func<IDictionary<string,CloudStorageAccount>> factory)
        {
            _storageAccounts = factory();
        }

        public IDictionary<string,CloudStorageAccount> LoadStorageAccounts()
        {
            return _storageAccounts;
        }
    }
}