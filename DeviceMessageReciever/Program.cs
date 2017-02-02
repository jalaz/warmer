using System;
using System.IO;
using Microsoft.ServiceBus.Messaging;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                            .AddUserSecrets("Warmer.DeviceSimulator")
                            .Build();

            var connectionString = config["WarmerQueueConnectionString"];
            
            var client = QueueClient.CreateFromConnectionString(connectionString);

            client.OnMessage(message => {
                Stream stream = message.GetBody<Stream>();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string messageBody = reader.ReadToEnd();
                Console.WriteLine($"Message body: ${messageBody}");
            });

            Console.ReadLine();
        }
    }
}
