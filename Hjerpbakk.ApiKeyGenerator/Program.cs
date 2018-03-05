using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Hjerpbakk.ApiKeyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));

            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] textBytes = encoding.GetBytes(config.App);
            Byte[] keyBytes = encoding.GetBytes(config.Password);

            HMACSHA256 hash = new HMACSHA256(keyBytes);
            Byte[] hashBytes = hash.ComputeHash(textBytes);

            var hashString = BitConverter.ToString(hashBytes);
            var readableString = hashString.Replace("-", "").ToLower();
            Console.WriteLine(readableString);
        }
    }
}
