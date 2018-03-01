using System;
using Newtonsoft.Json;

namespace Hjerpbakk.PoorMansServiceDiscovery.Model {
    public struct Service {
        [JsonConstructor]
        public Service(string name, string ip) {
            Name = name;
            IP = ip.TrimEnd('\n').Trim('"');
            LastSeen = DateTime.UtcNow;
        }

        public Service(string name, string ip, DateTime lastSeen) : this(name, ip) {
            LastSeen = lastSeen;
        }

        public string Name { get; }
		public string IP { get; }
        public DateTime LastSeen { get; }

        public override string ToString() => string.Format("[Service: Name={0}, IP={1}, LastSeen={2}]", Name, IP, LastSeen);
    }
}