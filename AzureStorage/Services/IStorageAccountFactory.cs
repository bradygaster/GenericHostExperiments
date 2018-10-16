using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;

namespace GenericHostExperiments.AzureStorage
{
    public interface IStorageAccountFactory
    {
        IDictionary<string,CloudStorageAccount> LoadStorageAccounts();
    }
}