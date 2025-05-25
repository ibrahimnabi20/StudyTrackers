using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace StudyTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Entry point: builds and runs the web host
            CreateHostBuilder(args).Build().Run();
        }

        // Configures the ASP.NET Core web host and sets Startup.cs as the startup class
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); // Reference to Startup.cs
                });
    }
}