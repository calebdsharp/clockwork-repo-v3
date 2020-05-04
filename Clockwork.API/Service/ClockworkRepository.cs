using System;
using System.Linq;
using Clockwork.API.Models;

namespace Clockwork.API.Service
{
    public class ClockWorkRepository
    {
        public static void InsertNewEntry(CurrentTimeQuery newEntry)
        {
            var db = new ClockworkContext();
            // insert new entry to database
            db.CurrentTimeQueries.Add(newEntry);
            Console.WriteLine("{0} records(s) saved to database", db.SaveChanges());
        }

        public static IOrderedQueryable<CurrentTimeQuery> GetAllTimeRequests()
        {
            // get all entries
            var db = new ClockworkContext();
            IOrderedQueryable<CurrentTimeQuery> timeQueries =
                from query in db.CurrentTimeQueries
                orderby query.CurrentTimeQueryId
                select query;

            return timeQueries;
        }
    }
}
