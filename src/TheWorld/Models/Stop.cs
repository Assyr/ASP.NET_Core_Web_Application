using System;

namespace TheWorld.Models
{
    public class Stop
    {
        public int ID { get; set; }
        public string Name { get; set; } //Name of a stop
        public double Latitude { get; set; } //Latitude of a single stop
        public double Longitude { get; set; } //Longitude of a single stop
        public int Order { get; set; } //Order of our stops
        public DateTime Arrival { get; set; }
    }
}