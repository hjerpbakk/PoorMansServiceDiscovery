using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hjerpbakk.PoorMansServiceDiscovery.Model;
using Hjerpbakk.PoorMansServiceDiscovery.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hjerpbakk.PoorMansServiceDiscovery.Controllers
{
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        readonly ServiceDiscoveryService serviceDiscoveryService;
        
        public ServicesController(ServiceDiscoveryService serviceDiscoveryService) {
            this.serviceDiscoveryService = serviceDiscoveryService;
        }

        [HttpGet]
        public async Task<IEnumerable<Service>> Get() => await serviceDiscoveryService.GetServices();

        [HttpGet("{serviceName}")]
        public async Task<Service> Get(string serviceName) {
            if (serviceName == null) {
                throw new ArgumentNullException(nameof(serviceName));    
            }

            return await serviceDiscoveryService.GetService(serviceName);
        } 

        [HttpPost]
        public async Task Post([FromBody]Service service) => await serviceDiscoveryService.Register(service);
    }
}
