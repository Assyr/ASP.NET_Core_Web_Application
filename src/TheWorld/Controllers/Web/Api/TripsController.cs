﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web.Api
{
    //Set up a base route for the entire class instead of specifying one for each function
    //Now we just specify whether they're Get or Post requests at the function level.
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private IWorldRepository _repository;

        public TripsController(IWorldRepository repository)
        {
            _repository = repository;
        }
        //Specify the route below to call our JsonResult which returns a trip with the name "My Trip"
        [HttpGet("")]
        public IActionResult Get()
        {
            /*if (true)
                return BadRequest("Bad things happened");*/ //Handling a bad request
            try
            {
                var results = _repository.GetAllTrips();
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));

            }
            catch (Exception ex)
            {
                // TODO Log the ex error.

                return BadRequest("Error Occurred");
            }
        }

       [HttpPost("")]//Bind the data in the body of the post request to 'theTrip' and more specifically... 'name' in 'theTrip' object as our POST request specifies
        public IActionResult Post([FromBody]TripViewModel theTrip)
        {
            //Check if the data being fed is valid - and only if it is.. return a created 201 code which is a success
            if(ModelState.IsValid)//ModelState.IsValid checks if what has been fed to theTrip fits the requires we set up in our 'TripViewModel'
            {
                //Save to the database, first we must map our TripViewModel into 'Trip'
                var newTrip = Mapper.Map<Trip>(theTrip); //Do the mapping using 'AutoMapper'



                //Using Created here to return a 201 status code 
                //provide a uri to create the page at and provide our value
                return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
            }

            return BadRequest(ModelState);
        }
    }
}
