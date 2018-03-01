using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hjerpbakk.PoorMansServiceDiscovery.Configuration;
using Hjerpbakk.PoorMansServiceDiscovery.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Hjerpbakk.PoorMansServiceDiscovery.Clients
{
    public class ServiceDiscoveryClient
    {
        const string ContainerName = "discovery";

        readonly CloudBlobClient blobClient;
        readonly CloudBlobContainer discoveryContainer;
        readonly HttpClient httpClient;

        public ServiceDiscoveryClient(IBlobStorageConfiguration blobStorageConfiguration, HttpClient httpClient)
        {
            var storageAccount = CloudStorageAccount.Parse(blobStorageConfiguration.BlobStorageConnectionString);

            blobClient = storageAccount.CreateCloudBlobClient();

            discoveryContainer = blobClient.GetContainerReference(ContainerName);
            discoveryContainer.CreateIfNotExistsAsync().GetAwaiter();

            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<CloudBlockBlob>> GetServices()
        {
            var token = new BlobContinuationToken();
            var blobs = await discoveryContainer.ListBlobsSegmentedAsync(token);
            return blobs.Results.Cast<CloudBlockBlob>();
        }

        public CloudBlockBlob GetService(string serviceName) => 
            discoveryContainer.GetBlockBlobReference(serviceName);

        public async Task Register(string serviceName, string serializedService) => 
            await GetService(serviceName).UploadTextAsync(serializedService);
    }
}