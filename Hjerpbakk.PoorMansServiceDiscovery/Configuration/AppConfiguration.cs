using Newtonsoft.Json;

namespace Hjerpbakk.PoorMansServiceDiscovery.Configuration
{
    public class AppConfiguration : IBlobStorageConfiguration, IClientConfiguration
    {
        public string BlobStorageConnectionString { get; set; }     
        public string[] ApiKeys { get; set; }
    }
}
