using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repos;
using Repos.Interfaces;
using Services;
using Services.Interfaces;

namespace MVC
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
            services.AddControllersWithViews();
            services.AddScoped(typeof(IShortUrlService), typeof(ShortUrlService));
            services.AddScoped(typeof(IShortUrlRepo), typeof(ShortUrlRepo));

            // Collation None for case sensitive queries
            var cs = new ConnectionString { Filename = "ShortUrlLightDb.db", Collation = new Collation("en-GB/None") };
            services.AddScoped<ILiteDatabase, LiteDatabase>(c => new LiteDatabase(cs));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "shortUrlDefaultAction",
                    pattern: "shorturl/{action=addurl}/{id?}",
                    defaults: new { controller = "shorturl", action = "addurl" }
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{*shortUrl}",
                    defaults: new { controller = "shorturl", action = "RedirectToUrl" });
            });
        }
    }
}
