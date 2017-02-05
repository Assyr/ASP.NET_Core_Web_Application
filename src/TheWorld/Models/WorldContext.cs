using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TheWorld.Models
{
    //This is going to represent the access to the database itself
    public class WorldContext : DbContext
    {
        public WorldContext()
        {
            //ctor
        }

        //Set up classes so we can execute LINQ queries against
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }
    }
}
