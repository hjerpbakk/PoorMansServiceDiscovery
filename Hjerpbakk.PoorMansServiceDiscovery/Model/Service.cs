namespace Hjerpbakk.PoorMansServiceDiscovery.Model
{
    public struct Service
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
    }
}