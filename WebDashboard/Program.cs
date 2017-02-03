using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets()
                .Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .ConfigureLogging(l => l.AddConsole(config.GetSection("Logging")))
                .ConfigureServices(s => {
                    s.AddSignalR(options => {
                        options.Hubs.EnableDetailedErrors = true;
                    });
                    s.AddRouting();
                })
                .Configure(app => {
                    app.UseRouter(r => {
                        r.MapGet("", async (request, response, routeData) => {
                            response.ContentType = "text/html";
                            await response.WriteAsync(File.ReadAllText("index.html"));
                        });

                        r.MapGet("trigger", async (request, response, routeData) => {
                            var connectionManager = (IConnectionManager)r.ServiceProvider.GetService(typeof(IConnectionManager));
                            var random = new Random();
                            
                            while(true){
                                await Task.Delay(1000);
                                connectionManager.GetHubContext<DeviceHub>().Clients.All.publishTemperature($"{{\"temperature\" : \"{random.Next(0, 100)}\"}}");
                            }
                        });
                    });

                    app.UseWebSockets();
                    app.UseSignalR();
                })
                .Build();

            host.Run();
        }
    }
}