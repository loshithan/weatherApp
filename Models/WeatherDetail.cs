using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NodaTime;
using NodaTime.Extensions;
using Newtonsoft.Json;
namespace weatherApp.Models
{
    public class WeatherDetail
    {

        public Coord Coord { get; set; }
        public Sys Sys { get; set; }
        public List<Weather> Weather { get; set; }
        public Main Main { get; set; }

        public string Name { get; set; }

        public int Visibility { get; set; }

        public Wind Wind { get; set; }

        [JsonProperty("dt")]
        public long LastUpdatedTimeUnix { get; set; }

        public string LastUpdatedTime { get; set; }

        public int Id { get; set; }




    }
    public class Coord
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    }

    public class Sys
    {
        public string Country { get; set; }

        public long Sunrise { get; set; }

        public long Sunset { get; set; }

        public string SunriseTime { get; set; }
        public string SunsetTime { get; set; }

      



        
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

        public double Temp_min { get; set; }

        public double Temp_max { get; set; }

        public int Humidity { get; set; }

    }

    public class Wind
    {
        public double Speed { get; set; }
        public double Deg { get; set; }
    }




}