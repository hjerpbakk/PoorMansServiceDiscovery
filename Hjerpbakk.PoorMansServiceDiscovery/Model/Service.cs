using Hjerpbakk.ServiceDiscoveryClient.Model;

namespace Hjerpbakk.PoorMansServiceDiscovery.Model
{
    public struct Service : IService
    {
        static readonly char[] servicePostfix;

        static Service() {
            const string ServicePostfixString = ".txt";
            servicePostfix = ServicePostfixString.ToCharArray();
        }

        public Service(string fileName, string ip) {
            Name = fileName.TrimEnd(servicePostfix);
            IP = ip.TrimEnd('\n').Trim('"');
        }

		public string Name { get; set; }
		public string IP { get; set; }

		public override string ToString()
		{
			return string.Format("[Service: Name={0}, IP={1}]", Name, IP);
		}
    }
}