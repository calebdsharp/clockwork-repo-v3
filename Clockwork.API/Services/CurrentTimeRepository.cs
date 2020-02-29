using System;
using Clockwork.API.Models;
using Newtonsoft.Json;

namespace Clockwork.API.Services
{
    public class CurrentTimeRepository
    {
            // Get api/currenttime
            public GetTime[] GetTimes(string timezone)
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
                var newEntry = new CurrentTimeQuery
                {
                    UTCTime = utcTime,
                    Time = time,
                    ClientIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                    TimeZone = parsedTimeZoneInfo.Id
                };

                // insert new entry to database
                ClockWorkRepository.InsertNewEntry(newEntry);

                // return new entry along with all other entries
                var returnData = new Dictionary<string, object>
            {
                { "newEntry", newEntry },
                { "allEntries", ClockWorkRepository.FetchAll() }
            };
                // convert returned data to JSON
                return Ok(JsonConvert.SerializeObject(returnData));

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
        }
    }