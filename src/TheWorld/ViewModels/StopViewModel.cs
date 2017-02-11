using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.ViewModels
{
    public class StopViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; } //Name of a stop

        public double Latitude { get; set; } //Latitude of a single stop
        public double Longitude { get; set; } //Longitude of a single stop

        [Required]
        public int Order { get; set; } //Order of our stops

        [Required]
        public DateTime Arrival { get; set; }
    }
}
