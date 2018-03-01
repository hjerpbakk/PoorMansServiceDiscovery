using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Hjerpbakk.PoorMansServiceDiscovery.Clients;
using Hjerpbakk.PoorMansServiceDiscovery.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Hjerpbakk.PoorMansServiceDiscovery.Services
{
    public class ServiceDiscoveryService
    {
        readonly ServiceDiscoveryClient serviceDiscoveryClient;
        readonly IMemoryCache memoryCache;

        public ServiceDiscoveryService(ServiceDiscoveryClient serviceDiscoveryClient, IMemoryCache memoryCache)
        {
            this.serviceDiscoveryClient = serviceDiscoveryClient;
            this.memoryCache = memoryCache;
        }

        public async Task<Service> GetService(string serviceName)
        {
            if (!memoryCache.TryGetValue(serviceName, out Service service)) {
                var cloudBlockBlob = serviceDiscoveryClient.GetService(serviceName);
                service = await GetService(cloudBlockBlob);
                memoryCache.Set(serviceName, service);
            }

            return service;
        }

        public async Task<IEnumerable<Service>> GetServices()
        {
            var cloudBlockBlobs = await serviceDiscoveryClient.GetServices();
            var services = new List<Service>();
            foreach (var blob in cloudBlockBlobs)
            {
                if (!memoryCache.TryGetValue(blob.Name, out Service service)) {
                    service = await GetService(blob);
                    memoryCache.Set(blob.Name, service);
                }

                services.Add(service);
            }

            return services;
        }

        public async Task Register(Service service)
        {
            var serializedService = SerializeService(service);
            await serviceDiscoveryClient.Register(service.Name, serializedService);
            memoryCache.Remove(service.Name);
            memoryCache.Set(service.Name, service);
        }

        async Task<Service> GetService(CloudBlockBlob cloudBlockBlob)
        {
            using (var memoryStream = new MemoryStream())
            {
                await cloudBlockBlob.DownloadToStreamAsync(memoryStream);
                var content = Encoding.UTF8.GetString(memoryStream.ToArray());
                var lines = content.Split(';');
                var address = lines[0];
                var lastSeen = DateTime.MinValue;
                if (lines.Length > 1)
                {
                    lastSeen = DateTime.Parse(lines[1]);
                }

                return new Service(cloudBlockBlob.Name, address, lastSeen);
            }
        }

        string SerializeService(Service service) => $"{service.IP};{service.LastSeen}";
    }
}
