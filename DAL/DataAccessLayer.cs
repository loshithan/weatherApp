using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using System.Runtime.Caching;
using weatherApp.Models;
using System.Threading.Tasks;


namespace weatherApp.DAL
{
    public class DataAccessLayer
    {
        private readonly ApiHelper _apiHelper;
        private readonly CacheServices _cacheServices;
        List<WeatherDetail> weatherList;


        public DataAccessLayer()
        {
            _apiHelper = new ApiHelper();
            _cacheServices = new CacheServices();
        }

        public  async Task<List<WeatherDetail>> GetWeatherData()
        {
            try
            {
                List<City> cities = ReadJsonFromFile();

                //derive city codes
                List<string> cityCodes = cities.Select(c => c.CityCode).ToList();





                string cityId = string.Join(",", cityCodes);
                string apiKey = _apiHelper.ApiKey;

                string endpoint = $"group?id={cityId}&units=metric&appid={apiKey}";

                


                if (_cacheServices.Get<List<WeatherDetail>>("WeatherData") is null)
                {
                    string jData = await _apiHelper.GetAsync(endpoint);


                    WeatherJson jsonList = JsonConvert.DeserializeObject<WeatherJson>(jData);
                    List<WeatherDetail> list = jsonList.List;
                    CultureInfo cultureInfo = new CultureInfo("en-US");

                    foreach (WeatherDetail weatherDetail in list)
                    {
                        //change the unix timestamp from weatherdetail to readable time
                        weatherDetail.Sys.SunriseTime = DateTimeOffset.FromUnixTimeSeconds(weatherDetail.Sys.Sunrise).ToString("h:mmtt");
                        weatherDetail.Sys.SunsetTime = DateTimeOffset.FromUnixTimeSeconds(weatherDetail.Sys.Sunset).ToString("h:mmtt");

                        //extract full month name from each weather detail
                        string fullMonthName = DateTimeOffset.FromUnixTimeSeconds(weatherDetail.LastUpdatedTimeUnix).DateTime.ToString("MMMM", cultureInfo);

                        //shorten month name
                        string shortMonth = fullMonthName.Substring(0, 3);
                        DateTime date = DateTimeOffset.FromUnixTimeSeconds(weatherDetail.LastUpdatedTimeUnix).DateTime;

                        //assign formatted datetime string to instance variable
                        weatherDetail.LastUpdatedTime = $"{date:h:mmtt},{shortMonth} {date.Day}";

                    }
                    _cacheServices.Set("WeatherData", list, DateTimeOffset.Now.AddMinutes(5));


                }

                              

                weatherList = _cacheServices.Get<List<WeatherDetail>>("WeatherData");

                              
                

                return weatherList;

            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., HTTP request or JSON deserialization errors)
                throw ex;
                return null;
            }
        }

        public List<City> ReadJsonFromFile()
        {
            
            string filePath = Constants.CityDataFile;

            // Read the JSON data from the file.


            StreamReader sr = new StreamReader(filePath);
            string jsonData = sr.ReadToEnd();
            sr.Close();

            // Deserialize the JSON data into a CityList object.

            CityList cityList = JsonConvert.DeserializeObject<CityList>(jsonData);
            List<City> cities = cityList.List;
            return cities;

        }

        public WeatherDetail GetWeaterDetail(int? code)
        {
            WeatherDetail weatherDetail = null;
            if (_cacheServices.Get<List<WeatherDetail>>("WeatherData") != null && code != null)
            {
                List<WeatherDetail> list =  _cacheServices.Get<List<WeatherDetail>>("WeatherData");
                weatherDetail = list.FirstOrDefault(weather => weather.Id == code);

            }

            return weatherDetail;


        }
    }
}
