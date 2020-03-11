using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class GenreDTO: IGenerateHATEOASLinks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();

        public void GenerateLinks(IUrlHelper urlHelper)
        {
            Links.Add(new Link(urlHelper.Link("getGenre", new { Id = Id }), "get-genre", method: "GET"));
            Links.Add(new Link(urlHelper.Link("putGenre", new { Id = Id }), "put-genre", method: "PUT"));
            Links.Add(new Link(urlHelper.Link("deleteGenre", new { Id = Id }), "delete-genre", method: "DELETE"));
        }

        public ResourceCollection<GenreDTO> GenerateLinksCollection<GenreDTO>(List<GenreDTO> dtos, IUrlHelper urlHelper)
        {
            var resourceCollection = new ResourceCollection<GenreDTO>(dtos);
            resourceCollection.Links.Add(new Link(urlHelper.Link("getGenres", new { }), rel: "self", method: "GET"));
            resourceCollection.Links.Add(new Link(urlHelper.Link("createGenre", new { }), rel: "create-genre", method: "POST"));
            return resourceCollection;
        }
    }
}
