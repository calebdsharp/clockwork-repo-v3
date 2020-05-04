using System;
using System.Linq;

namespace Clockwork.API.Models
{
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
        public IOrderedQueryable<CurrentTimeQuery> CurrentTimeQueries { get; set; }
    }

    public class AllQueriesModel
    {
        public CurrentTimeQuery CurrentTimeQuery { get; set; }
        public AllTimeQueries AllTimeQueries { get; set; }
    }
}
