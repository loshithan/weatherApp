using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace weatherApp.DAL
{
    public static class Constants
    {
        public static string BaseUrl = "http://api.openweathermap.org/data/2.5";

        public static string CityDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "Cities", "cities.json");
    }
}