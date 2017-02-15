using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    public class GeoCoordsService
    {
        private IConfigurationRoot _config;
        private ILogger<GeoCoordsService> _logger;

        public GeoCoordsService(ILogger<GeoCoordsService> logger,
            IConfigurationRoot config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<GeoCoordsResult> GetCoordAsync(string name)
        {
            //Initialize an instance of GeoCoordsResult
            var result = new GeoCoordsResult()
            {
                Success = false,
                Message = "Failed to get coordinates"
            };

            var apiKey = _config["Keys:BingKey"];
            var encodedName = WebUtility.UrlEncode(name);//Encode string for a url
            var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodedName}&key={apiKey}";

            //Create an instance of http client we will use to return our json
            var client = new HttpClient();

            //Return our json using the httpclient's 'GetStringAsync' method nd the url we constructed above
            var json = await client.GetStringAsync(url);

            //Using JObject, parse the json into results which we can then use to step through and check things
            var results = JObject.Parse(json);
            //Looks for the resources we searched for and stores them to 'resources'
            var resources = results["resourceSets"][0]["resources"];
            //If there are no results - we couldn't find the location
            if (!results["resourceSets"][0]["resources"].HasValues)
            {
                //Could not find place name
                result.Message = $"Could not find '{name}' as a location";
            }
            else //if resources are found
            {
                //store the property "confidence" of the resource(s) found which tells us how confident that the data it returned is for the location we specified
                var confidence = (string)resources[0]["confidence"];
                //If confidence is not high
                if (confidence != "High") // if this confidence value isn't high, don't use it and throw the following error message
                {
                    result.Message = $"Could not find a confident match for '{name}' as a location";
                }
                else //If it has high confidence, store the coordinates of the resources which is stored in 'geocodePoints:coordinates'
                {
                    //Return coordinates
                    var coords = resources[0]["geocodePoints"][0]["coordinates"];
                    //Access our GeoCoordsResult entity and set the following properties from a succesful request
                    result.Latitude = (double) coords[0];
                    result.Longitude = (double) coords[1];
                    result.Success = true;
                    result.Message = "Success";
                }
            }
            //Return our result instance to be displayed.
            return result;
        }
    }
}
