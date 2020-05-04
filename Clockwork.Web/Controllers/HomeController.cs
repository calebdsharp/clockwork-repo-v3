using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Clockwork.Web.Models;
using Newtonsoft.Json;

namespace Clockwork.Web.Controllers
{
    public class HomeController : Controller
    {
        private static HttpClient client;

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult AvailableTimezones()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            IReadOnlyCollection<TimeZoneInfo> timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo info in timeZoneInfos)
            {
                SelectListItem item = new SelectListItem() { Text = info.Id, Value = info.Id };
                items.Add(item);
            };

            var model = new AvailableTimezonesModel
            {
                Timezones = items
            };

            return PartialView("AvailableTimezones", model);
        }

        [HttpPost]
        public async Task<ActionResult> RequestedTimezone(string selectedTimezoneId)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");

            HttpContent content = new StringContent(JsonConvert.SerializeObject(selectedTimezoneId), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("api/requestedtimezone?timezone=" + selectedTimezoneId, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<AllQueriesModel>(responseContent);
            Console.WriteLine("Web Timezone: {0}", responseObject.CurrentTimeQuery.TimeZone);

            //var newEntry = new CurrentTimeQuery
            //{
            //    UTCTime = responseObject.CurrentTimeQuery.UTCTime,
            //    Time = responseObject.CurrentTimeQuery.Time,
            //    ClientIp = responseObject.CurrentTimeQuery.ClientIp,
            //    TimeZone = responseObject.CurrentTimeQuery.TimeZone
            //};

            //var allTimes = new AllTimeQueries
            //{
            //    CurrentTimeQueries = responseObject.AllTimeQueries.CurrentTimeQueries
            //};

            //var model = new AllQueriesModel
            //{
            //    CurrentTimeQuery = newEntry,
            //    AllTimeQueries = allTimes
            //};

            //return PartialView("RequestedTimezone", model);
            return Json(responseObject, JsonRequestBehavior.AllowGet);
        }

        // GET: lit of requested times
        public ActionResult GetRequestedTimes()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string response = client.GetStringAsync("api/requestedtimezone").Result;

            // deserialize JSON
            var allTimes = new JavaScriptSerializer().DeserializeObject(response);

            return Json(allTimes, JsonRequestBehavior.AllowGet);
            //return View(returnModel);
        }
    }
}

