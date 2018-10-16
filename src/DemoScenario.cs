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
    public class DemoAzureStorageQueueService : IHostedService
    {
        public DemoAzureStorageQueueService(ILogger<DemoQueueListenerService> logger,
            IStorageAccountFactory storageAccountFactory)
        {
            Logger = logger;
            StorageAccountFactory = storageAccountFactory;
        }

        public ILogger<DemoQueueListenerService> Logger { get; internal set; }
        public IStorageAccountFactory StorageAccountFactory { get; internal set; }
        public CloudStorageAccount StorageAccount { get; internal set; }
        public CloudQueue CloudQueue { get; internal set; }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Attaching to Azure Storage Queue");

            StorageAccountFactory.LoadStorageAccounts();
            StorageAccount = StorageAccountFactory.GetAccount("imageStorage");
            CloudQueue = StorageAccount.CreateCloudQueueClient().GetQueueReference("incoming");
            await CloudQueue.CreateIfNotExistsAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class DemoQueueListenerService : DemoAzureStorageQueueService
    {
        public DemoQueueListenerService(ILogger<DemoQueueListenerService> logger, 
            IStorageAccountFactory storageAccountFactory) : base(logger, storageAccountFactory)
        {
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);

            Logger.LogInformation("Listener Service Started");
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
                    Thread.Sleep(10000); // lame, but you get the idea
            }
        }
    }

    public class DemoQueueFeedService : DemoAzureStorageQueueService
    {
        public DemoQueueFeedService(ILogger<DemoQueueListenerService> logger, IStorageAccountFactory storageAccountFactory) : base(logger, storageAccountFactory)
        {
        }

        public Timer Timer { get; private set; }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);

            Logger.LogInformation("Feeder Service Started");
            
            Timer = new Timer(OnTimerTick, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private async void OnTimerTick(object state)
        {
            string msg = string.Format($"'Heartbeat time at {DateTime.UtcNow.ToString()}'.");
            Logger.LogInformation("SENDING message " + msg);
            await CloudQueue.AddMessageAsync(new CloudQueueMessage(msg));
        }
    }
}