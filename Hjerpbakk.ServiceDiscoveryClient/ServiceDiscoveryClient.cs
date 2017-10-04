using System.Net.Http;
using System.Threading.Tasks;
using Hjerpbakk.ServiceDiscoveryClient.Model;
using Newtonsoft.Json;

namespace Hjerpbakk.ServiceDiscoveryClient
{
    public class ServiceDiscoveryClient
    {
        readonly HttpClient httpClient;
        readonly string serviceDiscoveryURL;

        public ServiceDiscoveryClient(HttpClient httpClient, string serviceDiscoveryServerName)
        {
            this.httpClient = httpClient;
            serviceDiscoveryURL = "http://" + serviceDiscoveryServerName + "/api/services/";
        }

        public string FormatServiceURL(string ip) => "http://" + ip + "/api/";

        public async Task<string> GetServiceURL(string serviceName) {
			var service = await httpClient.GetStringAsync(serviceDiscoveryURL + serviceName);
			var serviceURL = JsonConvert.DeserializeObject<Service>(service).IP;
            return FormatServiceURL(serviceURL);
        }
	}
}
