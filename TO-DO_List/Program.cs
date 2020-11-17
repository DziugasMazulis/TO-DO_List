using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TO_DO_List.Contracts.Services;

namespace TO_DO_List
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            SeedDatabase(host);

            host.Run();
        }

        private static void SeedDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    //not the most efficient way to clear data but hey *shrugEmoji*
                    var databaseService = serviceProvider.GetRequiredService<IDatabaseService>();
                    databaseService.EnsureDeleted();
                    databaseService.EnsureCreated();
                    databaseService.SeedRoles();
                    databaseService.SeedUsers();
                    databaseService.SeedTasks();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
