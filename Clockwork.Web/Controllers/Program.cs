//using System;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using Clockwork.Web.Models;

//namespace Clockwork.Web.Controllers
//{
//    public class Program
//    {
//        static HttpClient client = new HttpClient();

//        static void ShowProduct(CurrentTimeView currentTime)
//        {
//            Console.WriteLine($"ID: {currentTime.CurrentTimeQueryId}\tIP: " +
//                $"{currentTime.ClientIp}\tTimeZone: {currentTime.TimeZone}");
//        }

//        static async Task<Uri> CreateProductAsync(CurrentTimeView currentTime)
//        {
//            HttpResponseMessage response = await client.PostAsJsonAsync(
//                "api/currenttime", currentTime);
//            response.EnsureSuccessStatusCode();

//            // return URI of the created resource.
//            return response.Headers.Location;
//        }

//        static async Task<CurrentTimeView> GetProductAsync(string path)
//        {
//            CurrentTimeView currentTime = null;
//            HttpResponseMessage response = await client.GetAsync(path);
//            if (response.IsSuccessStatusCode)
//            {
//                currentTime = await response.Content.ReadAsAsync<CurrentTimeView>();
//            }
//            return currentTime;
//        }

//        static void Main()
//        {
//            RunAsync().GetAwaiter().GetResult();
//        }

//        static async Task RunAsync()
//        {
//            // Update port # in the following line.
//            client.BaseAddress = new Uri("http://localhost:5000/");
//            client.DefaultRequestHeaders.Accept.Clear();
//            client.DefaultRequestHeaders.Accept.Add(
//                new MediaTypeWithQualityHeaderValue("application/json"));

//            try
//            {
//                // Create a new product
//                CurrentTimeView currentTime = new CurrentTimeView
//                {
//                    CurrentTimeQueryId = 12,
//                    ClientIp = "2",
//                    TimeZone = "America/Los_Angeles"
//                };

//                var url = await CreateProductAsync(currentTime);
//                Console.WriteLine($"Created at {url}");

//                // Get the product
//                currentTime = await GetProductAsync(url.PathAndQuery);
//                ShowProduct(currentTime);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message);
//            }

//            Console.ReadLine();
//        }
//    }
//}
