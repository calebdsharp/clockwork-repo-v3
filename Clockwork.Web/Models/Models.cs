using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Clockwork.Web.Models
{
    public class AvailableTimezonesModel
    {
        public IEnumerable<SelectListItem> Timezones { get; set; }
        public string SelectedTimezoneId { get; set; }
    }

    public class CurrentTimeQuery
    {
        public int CurrentTimeQueryId { get; set; }
        public DateTime Time { get; set; }
        public string ClientIp { get; set; }
        public DateTime UTCTime { get; set; }
        public string TimeZone { get; set; }
    }

    public class AllTimeQueries
    {
        public List<CurrentTimeQuery> CurrentTimeQueries { get; set; }
    }

    public class AllQueriesModel
    {
        public CurrentTimeQuery CurrentTimeQuery { get; set; }
        public AllTimeQueries AllTimeQueries { get; set; }
    }
}


