using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.WIC.BusinessLogic.Models;
using Com.WIC.BusinessLogic.Services;
using Com.WIC.Encoder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Com.WIC.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new Configuration();
            Configuration.Bind("WordsInContext", config);
            services.AddSingleton(config);
            services.AddSingleton(new StorageProviderService(_env.ContentRootPath));
            services.AddSingleton(new EncoderService(config.FfmpegBinPath));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=API}/{action=GetSupportedLanguages}/{id?}");
            });
        }
    }
}
