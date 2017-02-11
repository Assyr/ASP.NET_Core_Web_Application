using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private IHostingEnvironment _env;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, 
            IHostingEnvironment env,
            ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
            _env = env;
            
        }

        public void AddTrip(Trip trip)
        {
            //Add our trip to the context
            _context.Add(trip);
        }

        //Returns a list of our trips (read only)
        public IEnumerable<Trip> GetAllTrips()
        {
            if(_env.IsEnvironment("Development"))
                _logger.LogInformation("Getting all trips from _context (The database)");

            return _context.Trips.ToList();
        }

        public async Task<bool> SaveChangesAsync()
        {
            //push our data to the database (in this case it's whatever has been added via 'AddTrip' and return a bool value that indicates whether this succeeded or not
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
