using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class MovieTheaterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double DistanceInMeters { get; set; }
        public double DistanceInKms { get { return DistanceInMeters / 1000; } }
    }
}
