using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;

namespace TheWorld.Controllers.Web.Api
{
    public class TripsController : Controller
    {
        //Specify the route below to call our JsonResult which returns a trip with the name "My Trip"
        [HttpGet("api/trips")]
        public IActionResult Get()
        {
            /*if (true)
                return BadRequest("Bad things happened");*/ //Handling a bad request

            return Ok(new Trip() { Name = "My Trip" });
        }
    }
}
