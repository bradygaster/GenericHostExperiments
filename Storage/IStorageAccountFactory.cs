using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;

namespace GenericHostExperiments.Storage
{
    public interface IStorageAccountFactory
    {
        IDictionary<string,CloudStorageAccount> LoadStorageAccounts();
    }
}