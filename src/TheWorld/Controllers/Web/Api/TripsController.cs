using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;

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

            return Ok(_repository.GetAllTrips());
        }

       [HttpPost("")]//Bind the data in the body of the post request to 'theTrip' and more specifically... 'name' in 'theTrip' object as our POST request specifies
        public IActionResult Post([FromBody]Trip theTrip)
        {
            return Ok(true);
        }
    }
}
