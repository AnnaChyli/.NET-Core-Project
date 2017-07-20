using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using TheWorld.Models;
using TheWorld.Models.Repositories;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld
{
    public class Startup
    {
	    private IHostingEnvironment _env;
	    private IConfigurationRoot _config;

	    //For production purposes, the ctor with param has to be created to handle
	    public Startup(IHostingEnvironment env)
	    {
		    _env = env;
		    IConfigurationBuilder builder = new ConfigurationBuilder()
				.SetBasePath(_env.ContentRootPath)
				.AddJsonFile("config.json")
				.AddEnvironmentVariables();

		    _config = builder.Build();
	    }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
			//Register the configuration file as a singleton to supply its instance across the app
	        services.AddSingleton(_config);

			if (_env.IsEnvironment("Development") || _env.IsEnvironment("Testing"))
	        {
		        // Register our own service. Supply the IMailService interface.
		        //1 st way - Transient means that it will provide the instance of DebugMailService as 
		        //it needs it and keeping it cached around

		        //services.AddTransient<IMailService, DebugMailService>();

		        //2nd way - creates a new instance of DebugMailService for each set of requests reusing it during this request
		        services.AddScoped<IMailService, DebugMailService>();

		        //3rd way - create one instance of DebugMailService as needed passing it over and over again
		        //services.AddSingleton<IMailService, DebugMailService>();
	        }
	        else
	        {
				//TODO implement a real mail service
			}

			//Register Entity Framework context to work with DB
	        services.AddDbContext<WorldContext>();

			//Add dependency injection logic for IWorldRepository.. Will be created once per request cycle, coz it is expensive
	        services.AddScoped<IWorldRepository, WorldRepository>();

				//TRICK! - for test purposes
				//services.AddScoped<IWorldRepository, MockWorldRepository>();

			// Add prepopulated data
			services.AddTransient<WorldContextSeedData>();

	        services.AddLogging();

			//service container - to register all required services (class objects, interfaces,...). 
			//it uses dependency injection 
			//To setup a Camel case JSON notation for serialization
	        services.AddMvc()
		        .AddJsonOptions(config =>
				config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.   IHostingEnvironment env, ILoggerFactory loggerFactory
		public void Configure(IApplicationBuilder app, 
			IHostingEnvironment env, 
			WorldContextSeedData seeder,
			ILoggerFactory logFactory)
		{
			//Initialize Mapper
			Mapper.Initialize(config =>
				{
					//Create TripVM -> Trip and reverse map Trip -> TripVM
					config.CreateMap<TripViewModel, Trip>().ReverseMap();
					config.CreateMap<StopViewModel, Stop>().ReverseMap();

				}		
			);

			if (env.IsEnvironment("Development"))
			{
				//Allows to see 
				app.UseDeveloperExceptionPage();


				//Microsoft.Extensions.Logging
				logFactory.AddDebug(LogLevel.Information);
			}
			else
			{
				logFactory.AddDebug(LogLevel.Error);
			}

			//Add MiddleWare -> to serve files to the browser
			app.UseStaticFiles();

			//What routes belong to which controllers
			app.UseMvc(config =>
				{
					config.MapRoute(
						name: "Default",
						template: "{controller}/{action}/{id?}",
						defaults: new {controller = "App", action = "Index"}
					);
				}
			);

			// TRICK! - Configure() can not be async, so that's why async EnsureSeedData() has .Wait() 
			seeder.EnsureSeedData().Wait();



			//loggerFactory.AddConsole();

			//if (env.IsDevelopment())
			//{
			//    app.UseDeveloperExceptionPage();
			//}

			//app.Run(async (context) =>
			//{
			//    await context.Response.WriteAsync("<html><body><h2>Hello World!</h2></body></html>");
			//});
		}
    }
}
