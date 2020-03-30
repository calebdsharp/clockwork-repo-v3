using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Clockwork.Web.Models;

namespace Clockwork.Web.Controllers
{
    public class HomeController : Controller
    {
        private static HttpClient client;

        public ActionResult Index()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            IReadOnlyCollection<TimeZoneInfo> timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo info in timeZoneInfos)
            {
                SelectListItem item = new SelectListItem() { Text = info.Id, Value = info.Id };
                items.Add(item);
            }

            var model = new AllTimesViewModel
            {
                Timezones = items
            };

            return View(model);
        }

        // Post request for Timezone selection : passing selected value as query parameter
        [HttpPost]
        public async Task<ActionResult> Index(string SelectedTimezoneId)
        {

            client = new HttpClient();
            var urls = new string[]
            {
                "http://localhost:5000/api/currenttime?timezone=" + SelectedTimezoneId,
                "http://localhost:5000/api/alltimes"
            };

            var requests = urls.Select
                (
                    url => client.GetStringAsync(url)
                ).ToList();

            await Task.WhenAll(requests);

            var responses = requests.Select
                (
                    task => task.Result
                );


            var currentTime = responses.First();
            var currentTimeObject = new JavaScriptSerializer().Deserialize<CurrentTimeRequestModel>(currentTime);
            var allTimes = responses.Last();
            var allTimesObject = new JavaScriptSerializer().Deserialize<List<RequestedTimesModel>>(allTimes);

            var resultJson = new JavaScriptSerializer().Serialize(new { CurrentTimeRequestModel = currentTimeObject, RequestedTimesModel = allTimesObject });

            var jsonResponse = new JavaScriptSerializer().DeserializeObject(resultJson);

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        // GET: lit of requested times
        public ActionResult GetRequestedTimes()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string response = client.GetStringAsync("api/alltimes").Result;

            // deserialize JSON
            var alltimes = new JavaScriptSerializer().Deserialize<List<RequestedTimesModel>>(response);

            return Json(alltimes, JsonRequestBehavior.AllowGet);
        }
    }
}

