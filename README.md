# GenericHost Experiments

After some interesting discussion with my teammates I took a look at the docs for the .NET Core [`GenericHost`](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-2.1) object to develop some ideas on how I would use it, specifically for Azure scenarios. 

This repository includes some experimental extensions atop the `GenericHost` object. 

## Version 0.0.1.0

This first iteration resulted in adding an extension method to add [Azure Storage](https://docs.microsoft.com/en-us/azure/storage/common/storage-introduction) support. There are a variety of ways Azure Storage can be wired up using the experimental middleware:

### Via Configuration

THe default Azure Storage extension middleware assumes it should read all of the Azure Storage connection strings from configuration. As shown in `Program.cs`, the `UseAzureStorage()` extension method is used with no parameters. 

```csharp
var host = new HostBuilder()
    .UseAzureStorage();
```

This assumes your `appsettings.json` (or environment variables) have been set up with the following style configuration. 

```json
{
    "Azure": {
        "Storage": {
            "documentStorage": "DefaultEndpointsProtocol=https;AccountName=yourstorageaccount01;AccountKey=your_key;EndpointSuffix=core.windows.net",
            "imageStorage": "DefaultEndpointsProtocol=https;AccountName=yourstorageaccount02;AccountKey=your_key;EndpointSuffix=core.windows.net"
        }
    }
}
```

### Via Manual Setup

You can also setup your own accounts during configuration manually. 

```csharp
var host = new HostBuilder()
    .UseAzureStorage(() => { 
        CloudStorageAccount tmp;
        Dictionary<string,CloudStorageAccount> list = new Dictionary<string, CloudStorageAccount>();
        if(CloudStorageAccount.TryParse("DefaultEndpointsProtocol=https;AccountName=yourstorageaccount;AccountKey=YOUR_KEY;EndpointSuffix=core.windows.net", out tmp) && tmp != null)
            list.Add("ImageStorage", tmp);
        return list;
    })
    .Build();
```

### Via Service Implementation

The `IStorageAccountFactory` interface is provided so that you can construct your own implementations that load Azure Storage accounts in other means (say, using the Azure Management Libraries if you want access to all storage accounts in your subscription or in a specific resource group). 

An example implementation is included in this repository, shown below. It simply creates a single Storage Account from a connection string provided at construction. 

```csharp
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
```

Then, you can use the implementation during host build-up. 

```csharp
var host = new HostBuilder()
    .UseAzureStorage(new MyExampleIStorageAccountFactory("DefaultEndpointsProtocol=https;AccountName=yourstorageaccount;AccountKey=YOUR_KEY;EndpointSuffix=core.windows.net"))
    .Build();
```

## Feedback is welcome, contributions appreciated. 

If you think these types of extensions are useful, let us know what others you'd like to see. Submit some issues for new ideas, or send in a pull request if you have any additional ideas or extensions you think might be valuable. 

Happy Coding!