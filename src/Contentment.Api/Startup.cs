using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contentment.Api.Domain;
using Contentment.Api.Formatters;
using Contentment.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Contentment.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IApiInfoService, ApiInfoService>();
            services.AddTransient<IApiVersion, ApiVersion>();
            services.AddTransient<IContentService, ContentService>();
			services.AddTransient<IIdGenerator, GuidIdGenerator>();

            // Add framework services.
            services.AddMvc(
				mvcConfig => {
					mvcConfig.InputFormatters.OfType<JsonInputFormatter>().First().SupportedMediaTypes.Add(
						MediaTypeHeaderValue.Parse(ContentTypes.VENDOR_MIME_TYPE)
					);
				}
			)
			.AddMvcOptions(options => {
				options.OutputFormatters.Add(new VendorContentTypeJsonOutputFormatter());
			});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
