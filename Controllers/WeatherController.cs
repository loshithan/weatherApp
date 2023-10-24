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
using System.Web.UI;

namespace weatherApp.Controllers
{
    public class WeatherController : Controller
    {

        readonly IDataAccessLayer _dal;

        public WeatherController(IDataAccessLayer dal)
        {
            //instantiate data access layer
            _dal = dal;
        }
       




        // GET: Weather
        //action method to get data to home page

        //caching for 5mins stored in both server and client
        [OutputCache(Duration =300,Location = OutputCacheLocation.ServerAndClient)]
        public  async Task<ActionResult> GetData ()
        {
            var data = await _dal.GetWeatherData();

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


        public  ActionResult GetDetail(int? code)
        {
            WeatherDetail wd = _dal.GetWeatherDetailAsync(code);
            if (wd == null)
            {
                return RedirectToAction("Timeout", "Error");
            }

            return View(wd);


        }

        
    }
}