using System;
using System.Threading;
using System.Threading.Tasks;
using GenericHostExperiments.AzureStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace GenericHostExperiments
{
    public abstract class DemoAzureStorageQueueService : BackgroundService
    {
        public DemoAzureStorageQueueService(ILogger<DemoQueueListenerService> logger,
            IStorageAccountFactory storageAccountFactory)
        {
            Logger = logger;
            StorageAccountFactory = storageAccountFactory;
            StorageAccountFactory.LoadStorageAccounts();
            StorageAccount = StorageAccountFactory.GetAccount("imageStorage");
            CloudQueue = StorageAccount.CreateCloudQueueClient().GetQueueReference("incoming");
        }

        public ILogger<DemoQueueListenerService> Logger { get; internal set; }
        public IStorageAccountFactory StorageAccountFactory { get; internal set; }
        public CloudStorageAccount StorageAccount { get; internal set; }
        public CloudQueue CloudQueue { get; internal set; }
    }

    public class DemoQueueListenerService : DemoAzureStorageQueueService
    {
        public DemoQueueListenerService(ILogger<DemoQueueListenerService> logger, 
            IStorageAccountFactory storageAccountFactory) : base(logger, storageAccountFactory)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await CloudQueue.CreateIfNotExistsAsync();

            CloudQueueMessage msg = null;
            
            while (msg == null)
            {
                Logger.LogInformation("Checking for message...");
                msg = await CloudQueue.GetMessageAsync();

                if(msg != null)
                {
                    Logger.LogInformation("RECEIVED message: " + msg.AsString);
                    await CloudQueue.DeleteMessageAsync(msg);
                    msg = null;
                }
                else
                    await Task.Delay(TimeSpan.FromSeconds(10000), stoppingToken);
            }
        }
    }

    public class DemoQueueFeedService : DemoAzureStorageQueueService
    {
        public DemoQueueFeedService(ILogger<DemoQueueListenerService> logger, 
            IStorageAccountFactory storageAccountFactory) : base(logger, storageAccountFactory)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await CloudQueue.CreateIfNotExistsAsync();
            string msg = string.Format($"'Heartbeat time at {DateTime.UtcNow.ToString()}'.");
            Logger.LogInformation("SENDING message " + msg);
            await CloudQueue.AddMessageAsync(new CloudQueueMessage(msg));
            await Task.Delay(3000, stoppingToken);
        }
    }
}