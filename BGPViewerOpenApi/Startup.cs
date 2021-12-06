using System;
using System.IO;
using BGPViewerCore.Service;
using BGPViewerOpenApi.Model;
using BGPViewerOpenApi.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BGPViewerOpenApi
{
    public class Startup
    {
        private const int TIMEOUT = 7;
        private readonly string allowedOrigins = "_allowedOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy(
                    name: allowedOrigins,
                    builder => {
                        builder.WithOrigins(this.Configuration.GetSection("AllowedOrigins").Get<string[]>())
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                    }
                );
            });

            services.AddControllers();

            services.AddScoped<AsProvider>();
            services.AddScoped<ApiProvider>();
            services.AddScoped<AsProvider>();
            services.AddScoped<PrefixProvider>();
            services.AddScoped<IPAddressProvider>();
            services.AddScoped<SearchProvider>();

            services.AddScoped<ApiBase, BGPViewApi>();
            services.AddScoped<BGPViewerService>();
            services.AddScoped<IBGPViewerApi, BGPViewerWebApi>();

            services.AddScoped<ApiBase, BGPHeApi>();
            services.AddScoped<BGPHeService>((s) => new BGPHeService(s.GetService<IWebDriver>(), TIMEOUT));
            services.AddScoped<ChromeOptions>((s) => {
                var options = new ChromeOptions
                {
                    PageLoadStrategy = PageLoadStrategy.Eager,
                    LeaveBrowserRunning = false,
                    AcceptInsecureCertificates = true
                };
                options.AddArgument("headless");
                options.AddArgument("browser-test");
                options.AddArgument("bwsi");
                options.AddArgument("no-sandbox");
                options.AddArgument("disable-gpu");
                options.AddArgument("disable-crash-reporter");
                options.AddArgument("disable-extensions");
                options.AddArgument("disable-notifications");
                options.AddArgument("disable-in-process-stack-traces");
                options.AddArgument("disable-dev-shm-usage");
                options.AddArgument("disable-default-apps");
                options.AddArgument("log-level=1");
                options.AddArgument("verbose");
                options.SetLoggingPreference(LogType.Driver, LogLevel.All);
                options.SetLoggingPreference(LogType.Server, LogLevel.All);
                return options;
            });
            services.AddScoped<IWebDriver, ChromeDriver>((s) => new ChromeDriver(s.GetService<ChromeOptions>()));

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

            app.UseRouting();

            app.UseCors(allowedOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
