using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Clockwork.Web.Models;
using Newtonsoft.Json;


namespace Clockwork.Web.Controllers
{
    public class HomeController : Controller
    {
       
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: /Home/Welcome/ 
        public ActionResult Welcome(string name, int numTimes = 1)
        {
            ViewBag.Message = "Hello" + name;
            ViewBag.NumTimes = numTimes;

            return View();
        }
    }
}

