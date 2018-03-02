namespace Hjerpbakk.ApiKeyGenerator
{
    public struct Config
    {
        public Config(string password) {
            Password = password;  
        } 

        public string Password { get; }
    }
}
