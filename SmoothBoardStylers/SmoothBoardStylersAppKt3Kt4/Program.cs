using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmoothBoardStylersApp.Data;
using SmoothBoardStylersApp.Models;
using System;

namespace SmoothBoardStylersApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            try
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<Gebruiker>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var seeder = new ApplicationSeeder(userManager, roleManager);
                seeder.Seedusers().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
