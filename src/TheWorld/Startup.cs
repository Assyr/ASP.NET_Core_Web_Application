﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using TheWorld.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TheWorld
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            _config = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);

            if (_env.IsEnvironment("Development"))
            {
                //Add an interface    //And use the 'DebugMailService' class fulfill any of its needs
                services.AddScoped<IMailService, DebugMailService>();
                //The above means it will make an instance of DebugMailService as soon as it needs it but only use it in the scoped request
            }
            else
            {
                //Implement a real Mail Service (Research into this)
            }

            //Register entity framework and our specific context
            services.AddDbContext<WorldContext>();
            services.AddScoped<IWorldRepository, WorldRepository>();

            services.AddTransient<GeoCoordsService>(); //Transient doesn't re-use the provided service/class - creates a new one every time.

            //Interfaces allow us to adjust the implemention of the contents of the interface
            //for example, IWorldRepository specifies a method 'GetAllTrips' but doesn't say how to implement it
            //WorldRepository has a 'GetAllTrips' method that described the implementation - by using the interface we could supply a mock up version
            //and switch them out easily by changing 'WorldRepository' to some other class with the same methods

            //We have our WorldContextSeedData setup - but now we actually need to call it to push the data to the database so we have something to work with
            services.AddTransient<WorldContextSeedData>();//Construct it here so we can grab it in 'Configure' method

            //Adding identity as a service and configure the options via the config
            services.AddIdentity<WorldUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                //Where to direct the application for Login when authorization fails, we provided the following path - this will also store a return url so it knows where to bring the user back after they succesfully login
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                //This handles the events of our cookie...
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    //In the event of redirection to our login...(this overrides any cast of redirection to login)
                    OnRedirectToLogin = async ctx =>
                    {
                        //Check if we're dealing with the API - if we are we send a response with status code 401
                        if(ctx.Request.Path.StartsWithSegments("/api") &&//Does the request path start with /api - this tells us we're dealing with API
                        ctx.Response.StatusCode == 200) //Is the response code returning OKAY? we don't want to return 401 below if the status code is something else
                        {
                            ctx.Response.StatusCode = 401;
                        }
                        else
                        {
                            //If we're not dealing with API just redirect to the redirect uri like we usually do
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        await Task.Yield();//Let the direction complete..
                    }
                };
            })
            .AddEntityFrameworkStores<WorldContext>();

            services.AddLogging(); //Add logging

            services.AddMvc(config =>
            {
                if (_env.IsProduction())
                {
                    //Adds a filter so that if we attempt to go to an http, it's going to try to redirect to https
                    config.Filters.Add(new RequireHttpsAttribute());
                }
            })
            .AddJsonOptions(opt =>
            {
                //This ensure we're using camel cased for our Json requests (api) - making it easier for users to consume
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });//Hey, here register all the MVC services
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            WorldContextSeedData seeder,
            ILoggerFactory factory)//configure the middleware!
        {
            app.UseStaticFiles();

            //Turn identity on
            app.UseIdentity();

            Mapper.Initialize(config =>
            {   //Create a map between TripViewModel and Trip so we can use the AutoMapper functionality in our TripsController class to map the validated TripViewModel to Trip
                config.CreateMap<TripViewModel, Trip>().ReverseMap();
                config.CreateMap<StopViewModel, Stop>().ReverseMap();
            });

            //We want to avoid throwing sensitive exception information to the user
            //We can solve this with the following
            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage(); //For debugging.
                factory.AddDebug(LogLevel.Information);
            }
            else
            {
                factory.AddDebug(LogLevel.Error);
            }


            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",//Specify how our url template should be
                    defaults: new { controller = "App", action = "Index" } //And if one isn't provided, go grab our 'App' controller and use the 'Index' action
                    );               
            }); //Now use the MVC Services

            seeder.EnsureSeedData().Wait(); //Make our call to the EnsureSeedData method part of the 'WorldContextSeedData' class

        }
    }
}
