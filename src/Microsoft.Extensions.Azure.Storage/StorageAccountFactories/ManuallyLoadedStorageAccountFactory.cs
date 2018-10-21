using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;

namespace Microsoft.Extensions.Azure.Storage
{
    public class ManuallyLoadedStorageAccountFactory : IStorageAccountFactory
    {
        IDictionary<string,CloudStorageAccount> _storageAccounts;

        public ManuallyLoadedStorageAccountFactory(Func<IDictionary<string,CloudStorageAccount>> factory)
        {
            _storageAccounts = factory();
        }

        public CloudStorageAccount GetAccount(string name)
        {
            return _storageAccounts[name];
        }

        public IDictionary<string,CloudStorageAccount> LoadStorageAccounts()
        {
            return _storageAccounts;
        }
    }
}