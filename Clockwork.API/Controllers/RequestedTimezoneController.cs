using System;
using Microsoft.AspNetCore.Mvc;
using Clockwork.API.Models;
using Clockwork.API.Service;
using Newtonsoft.Json;

namespace Clockwork.API.Controllers
{
    [Route("api/[controller]")]
    public class RequestedTimezoneController : Controller
    {
        [HttpPost]
        public IActionResult PostTimezone(string timezone)
        {
            

            // Parse timezone get timezone name
            var parsedTimeZoneInfo = ParseTimeZone(timezone);
            if (parsedTimeZoneInfo == null)
            {
                return NotFound();
            }

            // Create new entry
            var utcTime = DateTime.UtcNow;
            // convert UTC time to local time
            var time = TimeZoneInfo.ConvertTimeFromUtc(utcTime, parsedTimeZoneInfo);
            var clientIp = this.HttpContext.Connection.RemoteIpAddress.ToString();

            // update CurrentTimeQuery
            var newEntry = new CurrentTimeQuery
            {
                UTCTime = utcTime,
                Time = time,
                ClientIp = clientIp,
                TimeZone = parsedTimeZoneInfo.Id,
            };

            // insert new entry to database
            ClockWorkRepository.InsertNewEntry(newEntry);

            // update AllTimeQueries
            var allTimes = new AllTimeQueries
            {
                CurrentTimeQueries = ClockWorkRepository.GetAllTimeRequests()
            };

            // add models to single model
            var model = new AllQueriesModel
            {
                CurrentTimeQuery = newEntry,
                AllTimeQueries = allTimes
            };

            Console.WriteLine(model.CurrentTimeQuery.TimeZone);

            var json = JsonConvert.SerializeObject(model);

            return Ok(json);
        }

        // pass timezone to method
        private TimeZoneInfo ParseTimeZone(string timezone)
        {
            try
            {   // get timezone by Id / location name 
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                Console.WriteLine("Time zone info: {0}", timezone);
                return timeZoneInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to parse timezone: {0}; Error: {1}", timezone, e);
                return null;
            }
        }

        [HttpGet]
        public IActionResult GetAllTimes()
        {
            //get all time from Clockwork Repo
            var allTimes = new AllTimeQueries
            {
                CurrentTimeQueries = ClockWorkRepository.GetAllTimeRequests()
            };

            var model = new AllQueriesModel
            {
                AllTimeQueries = allTimes
            };

            var json = JsonConvert.SerializeObject(model);

            var result = JsonConvert.DeserializeObject(json);

            return Ok(result);
        }
    }
}