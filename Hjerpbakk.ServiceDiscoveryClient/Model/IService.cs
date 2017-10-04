namespace Hjerpbakk.ServiceDiscovery.Client.Model
{
    public interface IService
    {
		string Name { get; set; }
		string IP { get; set; }
    }
}
