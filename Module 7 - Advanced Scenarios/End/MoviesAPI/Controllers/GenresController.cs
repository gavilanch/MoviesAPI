using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Filters;
using MoviesAPI.Helpers;
using MoviesAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenresController : CustomBaseController
    {
        private readonly ILogger<GenresController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenresController(ILogger<GenresController> logger,
            ApplicationDbContext context,
            IMapper mapper)
            :base(context, mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet(Name = "getGenres")] // api/genres
        [ServiceFilter(typeof(GenreHATEOASAttribute))]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            return await Get<Genre, GenreDTO>();

            //if (includeHATEOAS)
            //{
            //    var resourceCollection = new ResourceCollection<GenreDTO>(genresDTOs);
            //    genresDTOs.ForEach(genre => GenerateLinks(genre));
            //    resourceCollection.Links.Add(new Link(Url.Link("getGenres", new { }), rel: "self", method: "GET"));
            //    resourceCollection.Links.Add(new Link(Url.Link("createGenre", new { }), rel: "create-genre", method: "POST"));
            //    return Ok(resourceCollection);
            //}
        }

        //private void GenerateLinks(GenreDTO genreDTO)
        //{
        //    genreDTO.Links.Add(new Link(Url.Link("getGenre", new { Id = genreDTO.Id }), "get-genre", method: "GET"));
        //    genreDTO.Links.Add(new Link(Url.Link("putGenre", new { Id = genreDTO.Id }), "put-genre", method: "PUT"));
        //    genreDTO.Links.Add(new Link(Url.Link("deleteGenre", new { Id = genreDTO.Id }), "delete-genre", method: "DELETE"));
        //}

        [HttpGet("{Id:int}", Name = "getGenre")] // api/genres/example
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GenreDTO), 200)]
        [ServiceFilter(typeof(GenreHATEOASAttribute))]
        public async Task<ActionResult<GenreDTO>> Get(int Id)
        {
            return await Get<Genre, GenreDTO>(Id);
        }

        [HttpPost(Name = "createGenre")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Post([FromBody] GenreCreationDTO genreCreation)
        {
            return await Post<GenreCreationDTO, Genre, GenreDTO>(genreCreation, "getGenre");
        }

        [HttpPut("{id}", Name = "putGenre")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreationDTO genreCreation)
        {
            return await Put<GenreCreationDTO, Genre>(id, genreCreation);
        }

        /// <summary>
        /// Delete a genre
        /// </summary>
        /// <param name="id">Id of the genre to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "deleteGenre")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Genre>(id);
        }
    }
}
