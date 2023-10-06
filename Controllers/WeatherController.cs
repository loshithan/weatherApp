using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using weatherApp.Models;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using System.Runtime.Caching;


namespace weatherApp.Controllers
{
    public class WeatherController : Controller
    {
        HttpClient http = new HttpClient();

        //declare weatherlist to store results from webapi
        List<WeatherDetail> weatherList;
        readonly ObjectCache memoryCache;

        public WeatherController()
        {
            //instantiate memorycache
            memoryCache = MemoryCache.Default;
        }


        // GET: Weather
        //action method to get data to home page
        public  async Task<ActionResult> GetData ()
        {
            // load city details from cities.json file
            List<City> cities = ReadJsonFromFile();

            //derive city codes
            List<string> cityCodes = cities.Select(c => c.CityCode).ToList();
            




            string cityId = string.Join(",", cityCodes);

            //check memory cache has the key weatherdata
            if (!memoryCache.Contains("WeatherData"))
            {
                http.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");

                var response = await http.GetAsync($"group?id={cityId}&units=metric&appid=2341c79977d030e9747e08391ebfec28");
                string jsonData = await response.Content.ReadAsStringAsync();

                WeatherJson jsonList = JsonConvert.DeserializeObject<WeatherJson>(jsonData);
                weatherList = jsonList.List;

                // assign timeout as 5min
                var cacheEntryOptions = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5)
                };

                //add list to cache

                memoryCache.Add("WeatherData", weatherList, cacheEntryOptions);

            }

            //retrieve weatherlist from cache
            weatherList = memoryCache["WeatherData"] as List<WeatherDetail>;


            
            DateTimeOffset dto = new DateTimeOffset();
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(WeatherDetail weatherDetail in weatherList)
            {
                //change the unix timestamp from weatherdetail to readable time
                weatherDetail.Sys.SunriseTime = DateTimeOffset.FromUnixTimeSeconds(weatherDetail.Sys.Sunrise).ToString("h: mm tt");
                weatherDetail.Sys.SunsetTime = DateTimeOffset.FromUnixTimeSeconds(weatherDetail.Sys.Sunset).ToString("h: mm tt");

                //extract full month name from each weather detail
                string fullMonthName = DateTimeOffset.FromUnixTimeSeconds(weatherDetail.LastUpdatedTimeUnix).DateTime.ToString("MMMM", cultureInfo);

                //shorten month name
                string shortMonth = fullMonthName.Substring(0, 3);
                DateTime date = DateTimeOffset.FromUnixTimeSeconds(weatherDetail.LastUpdatedTimeUnix).DateTime;

                //assign formatted datetime string to instance variable
                weatherDetail.LastUpdatedTime = $"{date:h:mm tt},{shortMonth} {date.Day}";

            }






            return View(weatherList);
        }

        // Action method to retrieve weather detail for a specific city.

        public async Task<ActionResult> GetDetail(int? code)
        {
            WeatherDetail weatherDetail = null;
            if (memoryCache.Contains("WeatherData") && code!=null)
            {
                 List<WeatherDetail> list = memoryCache["WeatherData"] as List<WeatherDetail>;
                weatherDetail =list.FirstOrDefault(weather => weather.Id == code);

            }

            return View(weatherDetail);


        }
        // Method to read city details from a JSON file.

        public List<City> ReadJsonFromFile()
        {
            /*            string filePath = @"~/Contant/Cities/cities.json";
             *            
            */
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "Cities", "cities.json");

            // Read the JSON data from the file.


            StreamReader sr = new StreamReader(filePath);
            string jsonData = sr.ReadToEnd();
            sr.Close();

            // Deserialize the JSON data into a CityList object.

            CityList cityList = JsonConvert.DeserializeObject<CityList>(jsonData);
            List<City> cities = cityList.List;
            return cities;
            
        }
    }
}