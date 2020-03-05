using Microsoft.AspNetCore.Mvc;
using MoviesAPI.DTOs;
using MoviesAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers.V2
{
    [ApiController]
    [Route("api2")]
    //[HttpHeaderIsPresent("x-version", "2")]
    public class RootController: ControllerBase
    {
        [HttpGet(Name = "getRoot2")]
        public ActionResult<IEnumerable<Link>> Get()
        {
            List<Link> links = new List<Link>();

            links.Add(new Link(href: Url.Link("getRoot", new { }), rel: "self", method: "GET"));

            return links;
        }
    }
}
