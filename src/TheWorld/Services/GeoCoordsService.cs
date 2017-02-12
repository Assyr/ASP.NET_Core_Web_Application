using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    public class GeoCoordsService
    {
        private ILogger<GeoCoordsService> _logger;

        public GeoCoordsService(ILogger<GeoCoordsService> logger)
        {
            _logger = logger;
        }

        public async Task<GeoCoordsResult> GetCoordAsync(string name)
        {
            //Initialize an instance of GeoCoordsResult
            var result = new GeoCoordsResult()
            {
                Success = false,
                Message = "Failed to get coordinates"
            };

            var apiKey = "";
            var encodedName = WebUtility.UrlEncode(name);
            //var url = $
        }
    }
}
