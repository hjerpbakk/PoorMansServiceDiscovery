using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hjerpbakk.PoorMansServiceDiscovery.Configuration;
using Hjerpbakk.PoorMansServiceDiscovery.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace Hjerpbakk.PoorMansServiceDiscovery.Clients
{
    public class Client
    {
        const string ContainerName = "discovery";

        readonly CloudBlobClient blobClient;
        readonly CloudBlobContainer discoveryContainer;
        readonly HttpClient httpClient;

        readonly ConcurrentDictionary<string, Service> services;

        public Client(BlobStorageConfiguration blobStorageConfiguration, HttpClient httpClient)
		{
			var storageAccount = CloudStorageAccount.Parse(blobStorageConfiguration.ConnectionString);

			blobClient = storageAccount.CreateCloudBlobClient();

			discoveryContainer = blobClient.GetContainerReference(ContainerName);
			discoveryContainer.CreateIfNotExistsAsync().GetAwaiter();

            this.httpClient = httpClient;
            services = new ConcurrentDictionary<string, Service>();
        }

        public async Task<IEnumerable<Service>> GetServices() {
            var token = new BlobContinuationToken();
			var blobs = await discoveryContainer.ListBlobsSegmentedAsync(token);
            services.Clear();
            foreach (var blob in blobs.Results.Cast<CloudBlockBlob>())
            {
                var ip = "";
                using (var memoryStream = new MemoryStream())
                {
                	await blob.DownloadToStreamAsync(memoryStream);
                	ip = Encoding.UTF8.GetString(memoryStream.ToArray());
                }

                var service = new Service(blob.Name, ip);
                services.AddOrUpdate(service.Name, service, (a, b) => service);
            }

            return services.Values;
        }

        public Service GetService(string serviceName)
        {
            return services[serviceName];
        }

		// TODO: All service registring should go through this service
		public async Task PublishService(Service service) {
            services.AddOrUpdate(service.Name, service, (a, b) => service);
            var serviceArray = services.Values.Where(s => s.Name != "service-discovery-service" && s.Name != service.Name).ToArray();
			var jsonContent = JsonConvert.SerializeObject(service);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            foreach(var theService in serviceArray) {
                try
                {
                    await httpClient.PostAsync("http://" + theService.IP + "/api/services", content);
                }
                catch (Exception)
                {
					// TODO: What to do if crashing?
				}
                //var response = await httpClient.PostAsync("http://" + theService.IP + "/api/services", content);
			    //if (response.StatusCode == HttpStatusCode.OK)
			    //{
                //return;
			    //}

                //throw new Exception("An error occurred: \n" + response);
            }
        }
    }
}