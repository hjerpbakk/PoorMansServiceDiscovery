using System;
namespace Hjerpbakk.PoorMansServiceDiscovery.Configuration
{
    public interface IBlobStorageConfiguration
    {
        string BlobStorageConnectionString { get; }
    }
}
