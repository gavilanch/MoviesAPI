using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesAPI.DTOs;
using MoviesAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class RootController: ControllerBase
    {
        private readonly ILogger<RootController> logger;

        public RootController(ILogger<RootController> logger)
        {
            this.logger = logger;
        }

        [HttpGet(Name = "getRoot")]
        public ActionResult<IEnumerable<Link>> Get()
        {

            var iteration = 1;

            logger.LogDebug($"Debug {iteration}");
            logger.LogInformation($"Information {iteration}");
            logger.LogWarning($"Warning {iteration}");
            logger.LogError($"Error {iteration}");
            logger.LogCritical($"Critical {iteration}");

            try
            {
                throw new NotImplementedException();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            List<Link> links = new List<Link>();

            links.Add(new Link(href: Url.Link("getRoot", new { }), rel: "self", method: "GET"));
            links.Add(new Link(href: Url.Link("createUser", new { }), rel: "create-user", method: "POST"));
            links.Add(new Link(href: Url.Link("Login", new { }), rel: "login", method: "POST"));
            links.Add(new Link(href: Url.Link("getGenres", new { }), rel: "get-genres", method: "GET"));
            links.Add(new Link(href: Url.Link("getPeople", new { }), rel: "get-people", method: "GET"));

            return links;
        }
    }
}
