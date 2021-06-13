using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                identityDbContext.Database.Migrate();

                if (!userManager.Users.Any())
                {
                    userManager.CreateAsync(new AppUser()
                    {
                        UserName = "user1",
                        Email = "aosmanunal1@gmail.com"
                    }, "Password123*").Wait();

                    userManager.CreateAsync(new AppUser()
                    {
                        UserName = "user2",
                        Email = "aosmanunal2@gmail.com"
                    }, "Password123*").Wait();

                    userManager.CreateAsync(new AppUser()
                    {
                        UserName = "user3",
                        Email = "aosmanunal3@gmail.com"
                    }, "Password123*").Wait();

                    userManager.CreateAsync(new AppUser()
                    {
                        UserName = "user4",
                        Email = "aosmanunal4@gmail.com"
                    }, "Password123*").Wait();

                    userManager.CreateAsync(new AppUser()
                    {
                        UserName = "user5",
                        Email = "aosmanunal5@gmail.com"
                    }, "Password123*").Wait();
                }
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
