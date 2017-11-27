using Newtonsoft.Json;

namespace Hjerpbakk.PoorMansServiceDiscovery.Configuration
{
    public class AppConfiguration : IBlobStorageConfiguration
    {
        public string BlobStorageConnectionString { get; set; }            
    }
}
