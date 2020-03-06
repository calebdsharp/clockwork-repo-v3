using System;
using System.Linq;
using Clockwork.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Clockwork.API.Service;
using System.Net;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Clockwork.API.Controllers
{
    public class ReturnedDataController : Controller
    {
        //GET: api/returneddata
        [HttpGet("api/returneddata")]
        public IActionResult Get()
        {
            // get all entries to return
            IOrderedQueryable<CurrentTimeQuery> timeQueries =
            ClockWorkRepository.FetchAll();

            Console.WriteLine("Found {0} entries.", timeQueries.Count());
            // convert time queries to JSON
            return Ok(JsonConvert.SerializeObject(timeQueries));

        }
    }
}
