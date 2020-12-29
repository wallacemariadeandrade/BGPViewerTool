using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BGPViewerCore.Service;
using BGPViewerOpenApi.Model;
using BGPViewerOpenApi.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace BGPViewerOpenApi
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
            services.AddControllers();

            services.AddScoped<ApiProvider>();
            services.AddScoped<AsProvider>();
            services.AddScoped<PrefixProvider>();
            services.AddScoped<IPAddressProvider>();
            services.AddScoped<SearchProvider>();

            services.AddScoped<ApiBase, BGPViewApi>();
            services.AddScoped<BGPViewerService>();
            services.AddScoped<IBGPViewerApi, BGPViewerWebApi>();

            services.AddScoped<AsProvider>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Version = "v1",
                    Title = "BGPViewerOpenApi",
                    Description = "An API to provide an extensible and accessible information endpoint about Autonomous Systems (AS), prefixes and IP addresses.",
                    Contact = new OpenApiContact 
                    {
                        Name = "Wallace Andrade",
                        Email = "instrutorwallaceandrade@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/wallace-andrade-62414b128/"),
                    },
                    License = new OpenApiLicense 
                    {
                        Name = "MIT Licence",
                        Url = new Uri("https://github.com/wallacemariadeandrade/BGPViewerTool/tree/create-web-api/LICENCE")
                    }
                }); 

                c.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Error");

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BGPViewerOpenApi v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
