using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class FilterMovieTheatersDTO
    {
        [BindRequired]
        [Range(-90, 90)]
        public double Lat { get; set; }
        [BindRequired]
        [Range(-180, 180)]
        public double Long { get; set; }
        private int distanceInKms = 10;
        private int maxDistanceInKms = 50;
        public int DistanceInKms
        {
            get
            {
                return distanceInKms;
            }
            set
            {
                distanceInKms = (value > maxDistanceInKms) ? maxDistanceInKms : value;
            }
        }
    }
}
