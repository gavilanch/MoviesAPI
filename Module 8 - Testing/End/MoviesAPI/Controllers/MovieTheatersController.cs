using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/movietheaters")]
    public class MovieTheatersController: ControllerBase
    {
        private readonly ApplicationDbContext context;

        public MovieTheatersController(ApplicationDbContext applicationDbContext)
        {
            this.context = applicationDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieTheaterDTO>>> Get([FromQuery] FilterMovieTheatersDTO filterMovieTheatersDTO)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var usersLocation = geometryFactory
                .CreatePoint(new Coordinate(filterMovieTheatersDTO.Long, filterMovieTheatersDTO.Lat));

            var theaters = await context.MovieTheaters
                .OrderBy(x => x.Location.Distance(usersLocation))
                .Where(x => x.Location.IsWithinDistance(usersLocation, filterMovieTheatersDTO.DistanceInKms * 1000))
                .Select(x => new MovieTheaterDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    DistanceInMeters = Math.Round(x.Location.Distance(usersLocation))
                })
                .ToListAsync();

            return theaters;
        }
    }
}
