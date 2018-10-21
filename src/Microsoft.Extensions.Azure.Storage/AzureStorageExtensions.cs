using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.WindowsAzure.Storage;

namespace Microsoft.Extensions.Azure.Storage
{
    public static class AzureStorageExtensions
    {
        /// <summary>
        /// Builds the list of Azure Storage Accounts defined from configuration.
        /// </summary>
        public static IHostBuilder UseAzureStorage(this IHostBuilder builder)
        {
            return builder.ConfigureServices((services) => {
                services.AddSingleton<IStorageAccountFactory, ConfigurationFileStorageAccountFactory>();
                services.AddHostedService<AzureStorage>();
            });
        }

        /// <summary>
        /// Provides direct access to the underlying dictionary of storage accounts 
        /// so that they can be manually loaded during the host-building phase.
        /// </summary>
        public static IHostBuilder UseAzureStorage(this IHostBuilder builder, 
            Func<IDictionary<string, CloudStorageAccount>> factory)
        {
            return builder.ConfigureServices((services) => {
                services.AddSingleton<IStorageAccountFactory>(
                    new ManuallyLoadedStorageAccountFactory(factory));
                services.AddHostedService<AzureStorage>();
            });
        }

        /// <summary>
        /// Provides customization ability if you want to create your own
        /// IStorageAccountFactory implementation. 
        /// </summary>
        public static IHostBuilder UseAzureStorage(this IHostBuilder builder, 
            IStorageAccountFactory factory)
        {
            return builder.ConfigureServices((services) => {
                services.AddSingleton<IStorageAccountFactory>(factory);
                services.AddHostedService<AzureStorage>();
            });
        }

        /// <summary>
        /// Builds the list of Azure Storage Accounts defined from configuration.
        /// </summary>
        public static IWebHostBuilder UseAzureStorage(this IWebHostBuilder builder)
        {
            return builder.ConfigureServices((services) => {
                services.AddSingleton<IStorageAccountFactory, ConfigurationFileStorageAccountFactory>();
                services.AddHostedService<AzureStorage>();
            });
        }

        /// <summary>
        /// Provides direct access to the underlying dictionary of storage accounts 
        /// so that they can be manually loaded during the host-building phase.
        /// </summary>
        public static IWebHostBuilder UseAzureStorage(this IWebHostBuilder builder, 
            Func<IDictionary<string, CloudStorageAccount>> factory)
        {
            return builder.ConfigureServices((services) => {
                services.AddSingleton<IStorageAccountFactory>(
                    new ManuallyLoadedStorageAccountFactory(factory));
                services.AddHostedService<AzureStorage>();
            });
        }

        /// <summary>
        /// Provides customization ability if you want to create your own
        /// IStorageAccountFactory implementation. 
        /// </summary>
        public static IWebHostBuilder UseAzureStorage(this IWebHostBuilder builder, 
            IStorageAccountFactory factory)
        {
            return builder.ConfigureServices((services) => {
                services.AddSingleton<IStorageAccountFactory>(factory);
                services.AddHostedService<AzureStorage>();
            });
        }
    }
}