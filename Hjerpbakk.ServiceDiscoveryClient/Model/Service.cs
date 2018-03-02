namespace Hjerpbakk.ServiceDiscovery.Client.Model
{
    public struct Service
    {
        public Service(string name, string ip)
        {
            Name = name;
            IP = ip;
        }

        public string Name { get; set; }
		public string IP { get; set; }

		public override string ToString()
		{
			return string.Format("[Service: Name={0}, IP={1}]", Name, IP);
		}
    }
}
