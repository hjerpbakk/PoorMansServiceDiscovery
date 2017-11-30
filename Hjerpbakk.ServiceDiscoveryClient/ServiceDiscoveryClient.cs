using System.Net.Http;
using System.Threading.Tasks;
using Hjerpbakk.ServiceDiscovery.Client.Model;
using Newtonsoft.Json;

namespace Hjerpbakk.ServiceDiscovery.Client
{
    public class ServiceDiscoveryClient
    {
        readonly HttpClient httpClient;
        readonly string serviceDiscoveryURL;

        public ServiceDiscoveryClient(HttpClient httpClient, string serviceDiscoveryServerName)
        {
            this.httpClient = httpClient;
            serviceDiscoveryURL = serviceDiscoveryServerName + "/api/services/";
        }

        public string FormattedServiceURL(string ip) => ip + "/api/";

        public async Task<string> GetServiceURL(string serviceName) {
			var service = await httpClient.GetStringAsync(serviceDiscoveryURL + serviceName);
			var serviceURL = JsonConvert.DeserializeObject<Service>(service).IP;
            return FormattedServiceURL(serviceURL);
        }
	}
}
