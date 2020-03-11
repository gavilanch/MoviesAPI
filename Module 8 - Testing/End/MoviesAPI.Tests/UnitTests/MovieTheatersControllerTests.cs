using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoviesAPI.Controllers;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests.UnitTests
{
    [TestClass]
    public class MovieTheatersControllerTests : BaseTests
    {
        [TestMethod]
        public async Task GetMovieTheaters5KmsOrCloser()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            using (var context = LocalDbDatabaseInitializer.GetDbContextLocalDb())
            {
                var theaters = new List<MovieTheater>
                    {
                         new MovieTheater{Name = "Agora", Location = geometryFactory.CreatePoint(new Coordinate(-69.9388777, 18.4839233))},
                        new MovieTheater{Name = "Sambil", Location = geometryFactory.CreatePoint(new Coordinate(-69.9118804, 18.4826214))},
                        new MovieTheater{Name = "Megacentro", Location = geometryFactory.CreatePoint(new Coordinate(-69.856427, 18.506934))},
                        new MovieTheater{Name = "Village East Cinema", Location = geometryFactory.CreatePoint(new Coordinate(-73.986227, 40.730898))}
                    };

                context.AddRange(theaters);
                context.SaveChanges();
            }

            var filterMovieTheatersDTO = new FilterMovieTheatersDTO()
            {
                DistanceInKms = 5,
                Lat = 18.481139,
                Long = -69.938950
            };

            using (var context = LocalDbDatabaseInitializer.GetDbContextLocalDb())
            {
                var controller = new MovieTheatersController(context);
                var response = await controller.Get(filterMovieTheatersDTO);
                var theatersFromController = response.Value;
                Assert.AreEqual(2, theatersFromController.Count);
            }

        }
    }

}
