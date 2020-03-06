using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Clockwork.Web.Models
{
    //public class CurrentTimeView
    //{
    //    public int CurrentTimeQueryId { get; set; }
    //    public DateTime Time { get; set; }
    //    public string ClientIp { get; set; }
    //    public DateTime UTCTime { get; set; }
    //    public string TimeZone { get; set; }
    //}

    public class TimeItem
    {
        //Define the constructor of your PokeItem which is the same name as class, and is not returning anything.
        //Will take a string name, and url as a argument.
        public TimeItem(int id, string url)
        {
            CurrentTimeQueryId = id;
        }
        //Your Properties are auto-implemented.
        public int CurrentTimeQueryId { get; set; }
    }

    //Now define your asynchronous method which will retrieve all your pokemon.
    public static async void GetTime()
    {
        //Define your baseUrl
        string baseUrl = "hhttp://127.0.0.1:5000/api";
        //Have your using statements within a try/catch block
        try
        {
            //We will now define your HttpClient with your first using statement which will use a IDisposable.
            using (HttpClient client = new HttpClient())
            {
                //In the next using statement you will initiate the Get Request, use the await keyword so it will execute the using statement in order.
                using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                {
                    //Then get the content from the response in the next using statement, then within it you will get the data, and convert it to a c# object.
                    using (HttpContent content = res.Content)
                    {
                        //Now assign your content to your data variable, by converting into a string using the await keyword.
                        var data = await content.ReadAsStringAsync();
                        //If the data isn't null return log convert the data using newtonsoft JObject Parse class method on the data.
                        if (data != null)
                        {
                            //Now log your data in the console
                            Console.WriteLine("data------------{0}", data);
                        }
                        else
                        {
                            Console.WriteLine("NO Data----------");
                        }
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine("Exception Hit------------");
            Console.WriteLine(exception);
        }
    }

}
