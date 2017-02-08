using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Warmer.AuthorizationServer.Repositories;

namespace Warmer.AuthorizationServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseStartup<Startup>()
            //    .Build();

            //host.Run();
            Run().GetAwaiter().GetResult();
        }

        public static async Task Run()
        {
            var userRepo = new AzureTableUserRepository("UseDevelopmentStorage=true;", "users");
            await userRepo.Register(new User(Guid.NewGuid(), "some name", "super email"), "test");
        }
    }
}
