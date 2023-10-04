using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace weatherApp.Models
{
    public class City
    {
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string Temp { get; set; }
        public string Status { get; set; }
    }
}