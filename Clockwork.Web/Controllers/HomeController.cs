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
            //List<SelectListItem> items = new List<SelectListItem>();
            //IReadOnlyCollection<TimeZoneInfo> timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();
            //foreach (TimeZoneInfo info in timeZoneInfos)
            //{
            //    //var times = new SelectListGroup { Name = info.Id };
            //    SelectListItem item = new SelectListItem() { Text = info.Id, Value = info.Id };
            //    items.Add(item);
            //}

            //var model = new AllTimesViewModel
            //{
            //    Timezones = items
            //};
            //var model = new AllTimesViewModel();
            //model.CurrentTimeRequest = currentTimeRequestModel;

            //return View(model);
            return View();
        }

        public ActionResult TimezoneOptions()
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

            return PartialView("TimezoneOptions", model);
        }

        public ActionResult ShowCurrentTime(CurrentTimeRequestModel currentTimeRequestModel)
        {
            var model = new AllTimesViewModel();
            model.CurrentTimeRequest = currentTimeRequestModel;
            return PartialView("ShowCurrentTime", model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(string SelectedTimezoneId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    client = new HttpClient();
                    client.BaseAddress = new Uri("http://localhost:5000/");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage responseMessage = await client.GetAsync("http://localhost:5000/api/currenttime?timezone=" + SelectedTimezoneId);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var selectedTime = new JavaScriptSerializer().Deserialize<CurrentTimeRequestModel>(responseData);
                        if (selectedTime == null)
                        {
                            return HttpNotFound();
                        }
                        CurrentTimeRequestModel model = new CurrentTimeRequestModel();
                        model.TimeZone = SelectedTimezoneId;
                        model.Time = selectedTime.Time;

                        return RedirectToAction("Index", model);
                        //return RedirectToAction("GetCurrentTime", "Home", new { TimeZone = SelectedTimezoneId});
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ShowAllTimes()
        {
            var model = new AllTimesViewModel();
            model.RequestedTimes = GetRequestedTimes();

            return PartialView("ShowAllTimes", model);
        }

        // GET lit of requested times
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

