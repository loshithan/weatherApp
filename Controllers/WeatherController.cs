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
        List<WeatherDetail> weatherList;
        readonly ObjectCache memoryCache;

        public WeatherController()
        {
            memoryCache = MemoryCache.Default;
        }


        // GET: Weather
        public  async Task<ActionResult> GetData ()
        {
            List<City> cities = ReadJsonFromFile();
            List<string> cityCodes = cities.Select(c => c.CityCode).ToList();
            




            string cityId = string.Join(",", cityCodes);
            if (!memoryCache.Contains("WeatherData"))
            {
                http.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");

                var response = await http.GetAsync($"group?id={cityId}&units=metric&appid=2341c79977d030e9747e08391ebfec28");
                string jsonData = await response.Content.ReadAsStringAsync();

                WeatherJson jsonList = JsonConvert.DeserializeObject<WeatherJson>(jsonData);
                weatherList = jsonList.List;

                var cacheEntryOptions = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5)
                };

                memoryCache.Add("WeatherData", weatherList, cacheEntryOptions);

            }
            weatherList = memoryCache["WeatherData"] as List<WeatherDetail>;



            DateTimeOffset dto = new DateTimeOffset();
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(WeatherDetail weatherDetail in weatherList)
            {
                weatherDetail.Sys.SunriseTime = DateTimeOffset.FromUnixTimeSeconds(weatherDetail.Sys.Sunrise).ToString("h: mm tt");
                weatherDetail.Sys.SunsetTime = DateTimeOffset.FromUnixTimeSeconds(weatherDetail.Sys.Sunset).ToString("h: mm tt");
                string fullMonthName = DateTimeOffset.FromUnixTimeSeconds(weatherDetail.LastUpdatedTimeUnix).DateTime.ToString("MMMM", cultureInfo);
                string shortMonth = fullMonthName.Substring(0, 3);
                DateTime date = DateTimeOffset.FromUnixTimeSeconds(weatherDetail.LastUpdatedTimeUnix).DateTime;

                weatherDetail.LastUpdatedTime = $"{date:h:mm tt},{shortMonth} {date.Day}";

            }






            return View(weatherList);
        }
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

        public List<City> ReadJsonFromFile()
        {
            /*            string filePath = @"~/Contant/Cities/cities.json";
             *            
            */
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "Cities", "cities.json");

            StreamReader sr = new StreamReader(filePath);
            string jsonData = sr.ReadToEnd();
            sr.Close();
           CityList cityList = JsonConvert.DeserializeObject<CityList>(jsonData);
            List<City> cities = cityList.List;
            return cities;
            
        }
    }
}