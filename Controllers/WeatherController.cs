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
using weatherApp.DAL;


namespace weatherApp.Controllers
{
    public class WeatherController : Controller
    {

        //declare weatherlist to store results from webapi
        List<WeatherDetail> weatherList;
        DataAccessLayer dal;

        public WeatherController()
        {
            dal = new DataAccessLayer();
        }




        // GET: Weather
        //action method to get data to home page
        public  async Task<ActionResult> GetData ()
        {
           

           

            return View(await dal.GetWeatherData());
        }

        // Action method to retrieve weather detail for a specific city.

        public async Task<ActionResult> GetDetail(int? code)
        {
           

            return View(dal.GetWeaterDetail(code));


        }
        // Method to read city details from a JSON file.

        
    }
}