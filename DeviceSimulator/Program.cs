using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Devices.Client;

namespace DeviceSimulator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                            .AddUserSecrets("Warmer.DeviceSimulator")
                            .Build();

            Run(config).Wait();
            Console.WriteLine("message is successfuly sent");
        }

        public static async Task Run(IConfigurationRoot config)
        {
            var connectionString = config["WarmerConnectionString"];
            var client = DeviceClient.CreateFromConnectionString(connectionString);

            var content = new Message(Encoding.UTF8.GetBytes("testing..."));
            await client.SendEventAsync(content);
        }
    }
}
