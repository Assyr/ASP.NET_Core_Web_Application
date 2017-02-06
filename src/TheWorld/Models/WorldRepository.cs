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

        //Returns a list of our trips (read only)
        public IEnumerable<Trip> GetAllTrips()
        {
            if(_env.IsEnvironment("Development"))
                _logger.LogInformation("Getting all trips from _context (The database)");

            return _context.Trips.ToList();
        }
    }
}
