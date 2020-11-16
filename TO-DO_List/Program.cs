using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TO_DO_List.Contracts.Services;
using TO_DO_List.Data;
using TO_DO_List.Models;
using TO_DO_List.Services;

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
