using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TheWorld.Models
{
    //This is going to represent the access to the database itself
    public class WorldContext : DbContext
    {
        private IConfigurationRoot _config;

        //IConfigurationRoot allows us to access the config we set up in Startup.cs
        //which points to our 'config.json' - so we have access to this file now.
        public WorldContext(IConfigurationRoot config, DbContextOptions options) 
            : base(options)
        {
            //ctor
            _config = config;
        }

        //Set up classes so we can execute LINQ queries against
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //This overrides the configuration and allows us to tell it which SqlServer to use
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:WorldContextConnection"]);
        }
    }
}
