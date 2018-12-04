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
        public static IServiceCollection AddAzureStorage(this IServiceCollection services)
        {
            services.AddSingleton<IStorageAccountFactory, ConfigurationFileStorageAccountFactory>();
            services.AddHostedService<AzureStorage>();
            return services;
        }

        /// <summary>
        /// Provides direct access to the underlying dictionary of storage accounts 
        /// so that they can be manually loaded during the host-building phase.
        /// </summary>
        public static IServiceCollection AddAzureStorage(this IServiceCollection services, 
            Func<IDictionary<string, CloudStorageAccount>> factory)
        {
            services.AddSingleton<IStorageAccountFactory>(
                    new ManuallyLoadedStorageAccountFactory(factory));
            services.AddHostedService<AzureStorage>();
            return services;
        }

        /// <summary>
        /// Provides customization ability if you want to create your own
        /// IStorageAccountFactory implementation. 
        /// </summary>
        public static IServiceCollection AddAzureStorage(this IServiceCollection services, 
            IStorageAccountFactory factory)
        {
            services.AddSingleton<IStorageAccountFactory>(factory);
            services.AddHostedService<AzureStorage>();
            return services;
        }
    }
}