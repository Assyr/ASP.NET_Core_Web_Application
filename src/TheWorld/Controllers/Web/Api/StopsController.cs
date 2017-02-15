using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web.Api
{                   //The brace here refers to a parameter - tripName here links straight to 'string tripName' in our get method
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private GeoCoordsService _coordsService;
        private ILogger<StopsController> _logger;
        private IWorldRepository _repository;

        public StopsController(IWorldRepository repository, 
            ILogger<StopsController> logger,
            GeoCoordsService coordsService)
        {
            _repository = repository;
            _logger = logger;
            _coordsService = coordsService;
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

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                //First check if the data inputted into our StopViewModel from our Body is valid
                if(ModelState.IsValid)//This will check to see if the data entered meets the requirements we set up in 'StopViewModel.
                {
                    var newStop = Mapper.Map<Stop>(vm);

                    //Then look up the Geocodes of a new stop by calling our gecodeservice and returning the GeoCoordsResult instance
                    //with the supplied stop name
                    var result = await _coordsService.GetCoordAsync(newStop.Name);
                    if (!result.Success) //If the lookup wasn't succesful - we can check this with 'Success'
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {
                        //Adjust the stops latitude and longitude before pushing to the database
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;

                        //Save to the database
                        _repository.AddStop(tripName, newStop);

                        if (await _repository.SaveChangesAsync())
                        {
                            return Created($"/api/trips/{tripName}/stops/{newStop.Name}",
                                Mapper.Map<StopViewModel>(newStop));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new Stop: {0}", ex);
            }

            return BadRequest("Failed to save new stop");
        }
    }
}
