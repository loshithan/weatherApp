using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace weatherApp.Models
{
    public class WeatherDetail
    {

        public Coord Coord { get; set; }
        public Sys Sys { get; set; }
        public List<Weather> Weather { get; set; }
        public Main Main { get; set; }

        public string Name { get; set; }




    }
    public class Coord
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    }

    public class Sys
    {
        public string Country { get; set; }
    }

    public class Weather
    {
        public int Id { get; set; }
        public string Main { get; set; }

        public string Description { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
        public int Pressure { get; set; }
    }




}