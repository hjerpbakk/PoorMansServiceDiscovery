using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hjerpbakk.ServiceDiscovery.Client.Model;
using Newtonsoft.Json;

namespace Hjerpbakk.ServiceDiscovery.Client
{
    public class ServiceDiscoveryClient
    {
        readonly HttpClient httpClient;
        readonly string serviceDiscoveryURL;
        readonly string apiKey;

        public ServiceDiscoveryClient(HttpClient httpClient, string serviceDiscoveryServerName, string apiKey)
        {
            this.httpClient = httpClient;
            serviceDiscoveryURL = serviceDiscoveryServerName + "/api/services/";
            this.apiKey = "?apikey=" + apiKey;
        }

        public string FormattedServiceURL(string ip) => ip + "/api/";

        public async Task<string> GetServiceURL(string serviceName) {
            var service = await httpClient.GetStringAsync(serviceDiscoveryURL + serviceName + apiKey);
			var serviceURL = JsonConvert.DeserializeObject<Service>(service).IP;
            return FormattedServiceURL(serviceURL);
        }

        public async Task Heartbeat(Service service) {
            var serializedService = JsonConvert.SerializeObject(service);
            var content = new StringContent(serializedService, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(serviceDiscoveryURL + apiKey, content);
            response.EnsureSuccessStatusCode();
        }
	}
}
