﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hjerpbakk.PoorMansServiceDiscovery.Clients;
using Hjerpbakk.PoorMansServiceDiscovery.Configuration;
using Hjerpbakk.PoorMansServiceDiscovery.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Hjerpbakk.PoorMansServiceDiscovery
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
            services.AddMvc();
            services.AddMemoryCache();

            var blobStorageConfiguration = ReadBlobStorageConfig();
            var httpClient = new HttpClient {
                Timeout = TimeSpan.FromSeconds(15D)
            };
            var serviceDiscoveryClient = new ServiceDiscoveryClient(blobStorageConfiguration, httpClient);
            serviceDiscoveryClient.GetServices().GetAwaiter();
            services.AddSingleton<IBlobStorageConfiguration>(blobStorageConfiguration);
            services.AddSingleton(serviceDiscoveryClient);
            services.AddSingleton<ServiceDiscoveryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvc();
        }

        static AppConfiguration ReadBlobStorageConfig()
		{
			return JsonConvert.DeserializeObject<AppConfiguration>(File.ReadAllText("config.json"));
		}
    }
}
