using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Clockwork.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Clockwork.Web.Controllers
{
    public class HomeController : Controller
    {
        private static HttpClient client;

        public ActionResult RequestedTime(string timezone)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string response = client.GetStringAsync("http://localhost:5000/api/currenttime?timezone=" + timezone).Result;

            var requestedTime = new JavaScriptSerializer().Deserialize<RequestedTime>(response);

            ViewBag.Message = response;



            return View("Index");
        }


        // GET: api/alltimes
        public ActionResult Index()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string response = client.GetStringAsync("api/alltimes").Result;

            // deserialize JSON
            var alltimes = new JavaScriptSerializer().Deserialize<List<TimesList>>(response);
            // reverse List order
            var reverseAllTimes = Enumerable.Reverse(alltimes).ToList();
            // 
            var allTimesModel = reverseAllTimes.GroupBy(item => item.CurrentTimeQueryId).ToArray();

            return View(allTimesModel);
        }

    }
}

