using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hjerpbakk.PoorMansServiceDiscovery.Clients;
using Hjerpbakk.PoorMansServiceDiscovery.Model;
using Microsoft.AspNetCore.Mvc;

namespace Hjerpbakk.PoorMansServiceDiscovery.Controllers
{
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        readonly Client serviceDiscoveryClient;
        
        public ServicesController(Client serviceDiscoveryClient) {
            this.serviceDiscoveryClient = serviceDiscoveryClient;
        }

        [HttpGet]
        public async Task<IEnumerable<Service>> Get()
        {
            var services = await serviceDiscoveryClient.GetServices();
            return services;
        }

        [HttpGet("{serviceName}")]
        public Service Get(string serviceName)
        {
            return serviceDiscoveryClient.GetService(serviceName);
        }

        [HttpPost]
        public async Task Post([FromBody]Service service)
        {
            await serviceDiscoveryClient.PublishService(service);
        }
    }
}
