using System;
namespace Hjerpbakk.ServiceDiscoveryClient.Model
{
    public interface IService
    {
		string Name { get; set; }
		string IP { get; set; }
    }
}
