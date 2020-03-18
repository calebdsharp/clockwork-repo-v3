using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Clockwork.Web.Models;

namespace Clockwork.Web.Controllers
{
    public class CurrentTimeController : Controller
    {
        private static HttpClient client;

        [HttpPost]
        public ActionResult Index(string timezone)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string selectedTimeResponse = client.GetStringAsync("http://localhost:5000/api/currenttime?timezone=" + "America/Los_Angeles").Result;

            var selectedTime = new JavaScriptSerializer().Deserialize<List<CurrentTimeRequestModel>>(selectedTimeResponse);

            //ViewBag.Timezone = selectedTime.TimeZone.ToString();
            //ViewBag.Time = selectedTime.Time.ToString();

            var selectedTimeModel = selectedTime.GroupBy(item => item.CurrentTimeQueryId).ToArray();

            return View(selectedTimeModel);
        }
    }
}
