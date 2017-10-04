using System;
namespace Hjerpbakk.ServiceDiscoveryClient.Model
{
    public struct Service : IService
    {
		public string Name { get; set; }
		public string IP { get; set; }

		public override string ToString()
		{
			return string.Format("[Service: Name={0}, IP={1}]", Name, IP);
		}
    }
}
