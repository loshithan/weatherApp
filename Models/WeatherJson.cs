using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace weatherApp.Models
{
    public class WeatherJson
    {
       public List<WeatherDetail> List { get; set; }
        public  int Cnt { get; set; }
    }
}