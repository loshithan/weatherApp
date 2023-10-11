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

        readonly DataAccessLayer dal;

        public WeatherController()
        {
            //instantiate data access layer
            dal = new DataAccessLayer();
        }




        // GET: Weather
        //action method to get data to home page
        public  async Task<ActionResult> GetData ()
        {
            var data = await dal.GetWeatherData();

            if (data is List<WeatherDetail>)
            {
                return View("GetData",data);
            }
            else if (data is string)
            {
                  string result = (string)data;

                return RedirectToAction(result, "Error");   
            }





            return View("Error");
        }

        // Action method to retrieve weather detail for a specific city.

        public async Task<ActionResult> GetDetail(int? code)
        {
            WeatherDetail wd = dal.GetWeaterDetail(code);
            if (wd == null)
            {
                return RedirectToAction("Timeout", "Error");
            }

            return View(wd);


        }

        
    }
}