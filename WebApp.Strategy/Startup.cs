using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp.Strategy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Strategy.Repositories;

namespace WebApp.Strategy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IProductRepository>(sp =>
            {
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var context = sp.GetRequiredService<AppIdentityDbContext>();

                if (httpContextAccessor.HttpContext == null) return new ProductRepositorySqlServer(context);

                var claim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == Settings.ClaimDatabaseType);

                if (claim == null) return new ProductRepositorySqlServer(context);

                var databaseType = (DatabaseTypes)int.Parse(claim.Value);

                return databaseType switch
                {
                    DatabaseTypes.SqlServer => new ProductRepositorySqlServer(context),
                    DatabaseTypes.MongoDb => new ProductRepositoryMongoDb(Configuration),
                    _ => throw new NotImplementedException()
                };
            });
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
            });

            services.AddIdentity<AppUser, IdentityRole>(s =>
            {
                s.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
