using System;
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

            services.AddMvc();//Hey, here register all the MVC services
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)//configure the middleware!
        {
            //We want to avoid throwing sensitive exception information to the user
            //We can solve this with the following
            if(env.IsEnvironment("Development"))
                app.UseDeveloperExceptionPage(); //For debugging.

            app.UseStaticFiles();
            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",//Specify how our url template should be
                    defaults: new { controller = "App", action = "Index" } //And if one isn't provided, go grab our 'App' controller and use the 'Index' action
                    );               
            }); //Now use the MVC Services
        }
    }
}
