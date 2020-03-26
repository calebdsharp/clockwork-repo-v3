using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public ActionResult Index(string SelectedTimezoneId)
        {
            if (ModelState.IsValid)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:5000/");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string responseMessage = client.GetStringAsync("http://localhost:5000/api/currenttime?timezone=" + SelectedTimezoneId).Result;
                if (responseMessage != null)
                {
                    // create JSON object : selectedTime
                    var selectedTime = new JavaScriptSerializer().Deserialize<CurrentTimeRequestModel>(responseMessage);
                    if (selectedTime == null)
                    {
                        return HttpNotFound();
                    }
                    return Json(selectedTime, JsonRequestBehavior.AllowGet);
                }
            }
            return View();
        }

        //public ActionResult TimezoneOptions()
        //{
        //    List<SelectListItem> items = new List<SelectListItem>();
        //    IReadOnlyCollection<TimeZoneInfo> timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();
        //    foreach (TimeZoneInfo info in timeZoneInfos)
        //    {
        //        SelectListItem item = new SelectListItem() { Text = info.Id, Value = info.Id };
        //        items.Add(item);
        //    }

        //    var model = new AllTimesViewModel
        //    {
        //        Timezones = items
        //    };

        //    return PartialView("TimezoneOptions", model);
        //}

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
            //return reverseAllTimes;
        }
    }
}

