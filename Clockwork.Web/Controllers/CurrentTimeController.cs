//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Web;
//using System.Web.Mvc;
//using Clockwork.Web.Models;

//namespace Clockwork.Web.Controllers
//{
//    [Route("api/currenttime")]
//    public class CurrentTimeController : Controller
//    {
//        // GET: CurrentTime
//        [HttpGet]
//        public ActionResult Index()
//        { 
//            IEnumerable<CurrentTimeViewModel> currentTime = null;

//            using (var client = new HttpClient())
//            {
//                client.BaseAddress = new Uri("http://localhost:5000/api/");
//                //HTTP GET
//                var responseTask = client.GetAsync("currenttime");
//                responseTask.Wait();

//                var result = responseTask.Result;
//                if (result.IsSuccessStatusCode)
//                {
//                    var readTask = result.Content.ReadAsAsync<IList<CurrentTimeViewModel>>();
//                    readTask.Wait();

//                    currentTime = readTask.Result;
//                }
//                else //web api sent error response 
//                {
//                    //log response status here..

//                    currentTime = Enumerable.Empty<CurrentTimeViewModel>();

//                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
//                }
//            }
//            return View(currentTime);
//        }
//    }
//}
