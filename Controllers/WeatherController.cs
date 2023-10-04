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

namespace weatherApp.Controllers
{
    public class WeatherController : Controller
    {
        HttpClient http = new HttpClient();


        // GET: Weather
        public  async Task<ActionResult> GetData ()
        {
            List<City> cities = ReadJsonFromFile();
            List<string> cityCodes = cities.Select(c => c.CityCode).ToList();

            List<WeatherDetail> weatherList = new List<WeatherDetail>();

            string cityId = string.Join(",", cityCodes);

            http.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");

            var response =await http.GetAsync($"group?id={cityId}&units=metric&appid=2341c79977d030e9747e08391ebfec28");
            string jsonData = await response.Content.ReadAsStringAsync();

            WeatherJson jsonList = JsonConvert.DeserializeObject<WeatherJson>(jsonData);
            weatherList = jsonList.List;

                            




            return View(weatherList);
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