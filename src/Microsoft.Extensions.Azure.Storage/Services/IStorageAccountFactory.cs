using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;

namespace Microsoft.Extensions.Azure.Storage
{
    public interface IStorageAccountFactory
    {
        IDictionary<string,CloudStorageAccount> LoadStorageAccounts();
        CloudStorageAccount GetAccount(string name);
    }
}