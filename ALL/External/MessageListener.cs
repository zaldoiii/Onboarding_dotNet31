using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace External
{
    public class MessageListener : IHostedService, IDisposable
    {
        private readonly EventProcessorClient processor;
        private readonly ILogger _logger;

        public MessageListener(IConfiguration config, ILogger<MessageListener> logger)
        {
            _logger = logger;

            string topic = config.GetValue<string>("EventHub:EventHubNameTest");
            string azureContainername = config.GetValue<string>("AzureStorage:AzureContainerEventHubTest");
            string eventHubConn = config.GetValue<string>("EventHub:ConnectionString");
            string azStorageConn = config.GetValue<string>("AzureStorage:ConnectionString");
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            BlobContainerClient storageClient = new BlobContainerClient(azStorageConn, azureContainername);

            processor = new EventProcessorClient(storageClient, consumerGroup, eventHubConn, topic);

            processor.ProcessEventAsync += ProcessEventHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;
        }

        public async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            /* handle logic consume data here*/
            _logger.LogInformation(Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        public async Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            /* handle error here*/
            _logger.LogError(eventArgs.Exception.Message);
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await processor.StartProcessingAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await processor.StopProcessingAsync();
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MessageListernerService()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
