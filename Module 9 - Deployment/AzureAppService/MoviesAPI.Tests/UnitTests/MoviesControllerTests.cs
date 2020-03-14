using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MoviesAPI.Controllers;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests.UnitTests
{
    [TestClass]
    public class MoviesControllerTests : BaseTests
    {
        private string CreateTestData()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var genre = new Genre() { Name = "genre 1" };

            var movies = new List<Movie>()
            {
                new Movie(){Title = "Movie 1", ReleaseDate = new DateTime(2010, 1,1), InTheaters = false},
                new Movie(){Title = "Future Movie", ReleaseDate = DateTime.Today.AddDays(1), InTheaters = false},
                new Movie(){Title = "In Theaters Movie", ReleaseDate = DateTime.Today.AddDays(-1), InTheaters = true}
            };

            var movieWithGenre = new Movie()
            {
                Title = "Movie With Genre",
                ReleaseDate = new DateTime(2010, 1, 1),
                InTheaters = false
            };
            movies.Add(movieWithGenre);

            context.Add(genre);
            context.AddRange(movies);
            context.SaveChanges();

            var movieGenre = new MoviesGenres() { GenreId = genre.Id, MovieId = movieWithGenre.Id };
            context.Add(movieGenre);
            context.SaveChanges();

            return databaseName;
        }

        [TestMethod]
        public async Task FilterByTitle()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var controller = new MoviesController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new FilterMoviesDTO()
            {
                Title = "Movie 1",
                RecordsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var movies = response.Value;
            Assert.AreEqual(1, movies.Count);
            Assert.AreEqual("Movie 1", movies[0].Title);
        }

        [TestMethod]
        public async Task FilterByInTheaters()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var controller = new MoviesController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new FilterMoviesDTO()
            {
                InTheaters = true,
                RecordsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var movies = response.Value;
            Assert.AreEqual(1, movies.Count);
            Assert.AreEqual("In Theaters Movie", movies[0].Title);
        }

        [TestMethod]
        public async Task FilterByUpcomingReleases()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var controller = new MoviesController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new FilterMoviesDTO()
            {
                UpcomingReleases = true,
                RecordsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var movies = response.Value;
            Assert.AreEqual(1, movies.Count);
            Assert.AreEqual("Future Movie", movies[0].Title);
        }

        [TestMethod]
        public async Task FilterByGenre()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var genreId = context.Genres.Select(x => x.Id).First();

            var context2 = BuildContext(databaseName);

            var controller = new MoviesController(context2, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new FilterMoviesDTO()
            {
                GenreId = genreId,
                RecordsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var movies = response.Value;
            Assert.AreEqual(1, movies.Count);
            Assert.AreEqual("Movie With Genre", movies[0].Title);
        }

        [TestMethod]
        public async Task FilterOrderByTitleAscending()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var controller = new MoviesController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new FilterMoviesDTO()
            {
                OrderingField = "title",
                AscendingOrder = true,
                RecordsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var movies = response.Value;

            var context2 = BuildContext(databaseName);
            var moviesFromDb = context2.Movies.OrderBy(x => x.Title).ToList();

            Assert.AreEqual(moviesFromDb.Count, movies.Count);
            for (int i = 0; i < moviesFromDb.Count; i++)
            {
                var movieFromController = movies[i];
                var movieFromDb = moviesFromDb[i];

                Assert.AreEqual(movieFromDb.Id, movieFromController.Id);
            }
        }

        [TestMethod]
        public async Task FilterOrderByTitleDescending()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var controller = new MoviesController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new FilterMoviesDTO()
            {
                OrderingField = "title",
                AscendingOrder = false,
                RecordsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var movies = response.Value;

            var context2 = BuildContext(databaseName);
            var moviesFromDb = context2.Movies.OrderByDescending(x => x.Title).ToList();

            Assert.AreEqual(moviesFromDb.Count, movies.Count);
            for (int i = 0; i < moviesFromDb.Count; i++)
            {
                var movieFromController = movies[i];
                var movieFromDb = moviesFromDb[i];

                Assert.AreEqual(movieFromDb.Id, movieFromController.Id);
            }
        }

        [TestMethod]
        public async Task FilterOrderyByWrongFieldStillReturnsMovies()
        {
            var databaseName = CreateTestData();
            var mapper = BuildMap();
            var context = BuildContext(databaseName);

            var mock = new Mock<ILogger<MoviesController>>();
            var controller = new MoviesController(context, mapper, null, mock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filterDTO = new FilterMoviesDTO()
            {
                OrderingField = "abcd",
                AscendingOrder = false,
                RecordsPerPage = 10
            };

            var response = await controller.Filter(filterDTO);
            var movies = response.Value;

            var context2 = BuildContext(databaseName);
            var moviesFromDb = context2.Movies.ToList();
            Assert.AreEqual(moviesFromDb.Count, movies.Count);
            Assert.AreEqual(1, mock.Invocations.Count);

        }
    }
}
