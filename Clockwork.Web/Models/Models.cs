using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Clockwork.Web.Models
{
    public class RequestedTimesModel
    {
        public int CurrentTimeQueryId { get; set; }
        public DateTime Time { get; set; }
        public string ClientIp { get; set; }
        public DateTime UTCTime { get; set; }
        public string TimeZone { get; set; }
    }

    public class CurrentTimeRequestModel
    {
        public int CurrentTimeQueryId { get; set; }
        public DateTime Time { get; set; }
        public string ClientIp { get; set; }
        public DateTime UTCTime { get; set; }
        public string TimeZone { get; set; }
    }

    public class AllTimesViewModel
    {
        public CurrentTimeRequestModel CurrentTimeRequest { get; set; }
        public List<RequestedTimesModel> RequestedTimes { get; set; }
    }
}


