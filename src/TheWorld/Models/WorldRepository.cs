﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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

        public void AddStop(string tripName, Stop newStop, string username)
        {
            //Grab our trip by the name supplied and the username that owns that trip
            var trip = GetUserTripByName(tripName, username);

            //This is to ensure we are getting the correct trip^  and below we can add our stop to it

            //If it exists..
            if(trip != null)
            {
                //Add our stop to this trip (foreign key is being set in our Stops table to indicate which trip it belongs to (trip id))
                trip.Stops.Add(newStop);
                //And here we are actually adding the stop to our stops table
                _context.Stops.Add(newStop);
            }
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

        public Trip GetTripByName(string tripName)
        {
            //Using our context, access our Trips and find the trip that matches our tripName and return it
            return _context.Trips
                .Include(t => t.Stops) //This line here adds the stops that are parrt of the trip we're trying to find to the trip. So we can access the trip that's returned and get access to the stops and not just the trip itself
                .Where(t => t.Name == tripName)
                .FirstOrDefault();
        }

        public IEnumerable<Trip> GetTripsByUsername(string name)
        {
            //Return all trips where the UserName is equal to the name provided
            return _context
                .Trips
                .Include(t => t.Stops) //get the stops too that are part of these trips too
                .Where(t => t.UserName == name)
                .ToList();
            //We don't use include here like we did with GetTripByName because we just want the names that are
            //being passed back and not the entire object
        }

        public Trip GetUserTripByName(string tripName, string username)
        {
            //Using our context, access our Trips and find the trip that matches our tripName and return it
            return _context.Trips
                .Include(t => t.Stops) //This line here adds the stops that are parrt of the trip we're trying to find to the trip. So we can access the trip that's returned and get access to the stops and not just the trip itself
                .Where(t => t.Name == tripName && t.UserName == username) //We do the username check so that we get the trip name specific to the user - users might have the same trip names and we don't want to return these to random users
                .FirstOrDefault();
        }

        public async Task<bool> SaveChangesAsync()
        {
            //push our data to the database (in this case it's whatever has been added via 'AddTrip' and return a bool value that indicates whether this succeeded or not
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
