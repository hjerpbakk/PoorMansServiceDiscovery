namespace Hjerpbakk.ApiKeyGenerator
{
    public struct Config
    {
        public Config(string app, string password) {
            App = app;
            Password = password;  
        } 

        public string App { get; }
        public string Password { get; }
    }
}
