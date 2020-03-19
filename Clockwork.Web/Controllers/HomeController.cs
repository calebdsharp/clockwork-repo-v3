using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Data.Entity;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Clockwork.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Clockwork.Web.Controllers
{
    public class HomeController : Controller
    {

        private static HttpClient client;

        // BEFORE
        //public ActionResult Index()
        //{
        //    List<SelectListItem> items = new List<SelectListItem>();
        //    IReadOnlyCollection<TimeZoneInfo> timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();
        //    foreach (TimeZoneInfo info in timeZoneInfos)
        //    {
        //        var times = new SelectListGroup { Name = info.Id };
        //        SelectListItem item = new SelectListItem() { Text = info.Id, Value = info.Id, Group = times };
        //        items.Add(item);
        //    }

        //    ViewBag.Timezones = items.ToArray();


        //    client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:5000/");

        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    string response = client.GetStringAsync("api/alltimes").Result;

        //    // deserialize JSON
        //    var alltimes = new JavaScriptSerializer().Deserialize<List<RequestedTimesModel>>(response);
        //    // reverse List order
        //    var reverseAllTimes = Enumerable.Reverse(alltimes).ToList();
        //    // add reverse list to model
        //    var allTimesModel = reverseAllTimes.GroupBy(item => item.CurrentTimeQueryId).ToArray();

        //    string selectedTimeResponse = client.GetStringAsync("http://localhost:5000/api/currenttime?timezone=" + "America/Detroit").Result;

        //    var selectedTime = new JavaScriptSerializer().Deserialize<CurrentTimeRequestModel>(selectedTimeResponse);

        //    ViewBag.Timezone = selectedTime.TimeZone.ToString();
        //    ViewBag.Time = selectedTime.Time.ToString();


        //    return View(allTimesModel);
        //}

        // AFTER
        public ActionResult Index()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            IReadOnlyCollection<TimeZoneInfo> timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo info in timeZoneInfos)
            {
                //var times = new SelectListGroup { Name = info.Id };
                SelectListItem item = new SelectListItem() { Text = info.Id, Value = info.Id };
                items.Add(item);
            }

            var model = new AllTimesViewModel
            {
                Timezones = items
            };

            //model.CurrentTimeRequest = GetCurrentTime();
            model.RequestedTimes = GetRequestedTimes();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string SelectedTimezoneId, AllTimesViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    client = new HttpClient();
                    client.BaseAddress = new Uri("http://localhost:5000/");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string selectedTimeResponse = client.GetStringAsync("http://localhost:5000/api/currenttime?timezone=" + SelectedTimezoneId).Result;

                    var selectedTime = new JavaScriptSerializer().Deserialize<CurrentTimeRequestModel>(selectedTimeResponse);

                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        //public CurrentTimeRequestModel GetCurrentTime()
        //{
        //    client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:5000/");

        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    string selectedTimeResponse = client.GetStringAsync("http://localhost:5000/api/currenttime?timezone=" + "America/New_York").Result;

        //    var selectedTime = new JavaScriptSerializer().Deserialize<CurrentTimeRequestModel>(selectedTimeResponse);

        //    CurrentTimeRequestModel model = new CurrentTimeRequestModel()
        //    {
        //        CurrentTimeQueryId = selectedTime.CurrentTimeQueryId,
        //        Time = selectedTime.Time,
        //        ClientIp = selectedTime.ClientIp,
        //        UTCTime = selectedTime.UTCTime,
        //        TimeZone = selectedTime.TimeZone
        //    };

        //    return model;
        //}

        public List<RequestedTimesModel> GetRequestedTimes()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string response = client.GetStringAsync("api/alltimes").Result;

            // deserialize JSON
            var alltimes = new JavaScriptSerializer().Deserialize<List<RequestedTimesModel>>(response);
            // reverse List order
            var reverseAllTimes = Enumerable.Reverse(alltimes).ToList();

            return reverseAllTimes;
        }
    }
}

