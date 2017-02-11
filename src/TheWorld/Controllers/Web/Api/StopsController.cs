using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web.Api
{                   //The brace here refers to a parameter - tripName here links straight to 'string tripName' in our get method
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private ILogger<StopsController> _logger;
        private IWorldRepository _repository;

        public StopsController(IWorldRepository repository, ILogger<StopsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName); //Here we have returned our direct 'Trip' entity of the tripName specified, but I don't want to pass this to the user..

                //Since we will be accessing the Stops of our trip, we use our StopViewModel to map our collection of stops to a list of 'IEnumerable StopViewModel's'
                //What I want to pass to the user is the StopViewModels of our 'Trip' Stops so we don't give them the guts of it all and only what we want them to see.

                //Map our trip.Stops collection to an 'IEnumerable' of 'StopViewModel'
                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList())); //Return our stops ordered via the 'Order' column
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get stops: {0}", ex);
            }

            return BadRequest("Failed to get stops");
        }
    }
}
