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
using weatherApp.CustomException;

namespace weatherApp.DAL
{
    public class DataAccessLayer
    {
        private readonly ApiHelper _apiHelper;
        private readonly CacheServices _cacheServices;
        List<WeatherDetail> weatherList;


        public DataAccessLayer()
        {
            //instantiate apihelper
            _apiHelper = new ApiHelper();

            //instantiate cacheservice 
            _cacheServices = new CacheServices();
        }

        //get weather data returns list of weather detail obj or exception string
        public  async Task<object> GetWeatherData()
        {
            try
            {
                List<City> cities = ReadJsonFromFile();

                //derive city codes
                List<string> cityCodes = cities.Select(c => c.CityCode).ToList();





                string cityId = string.Join(",", cityCodes);

                //derive api key using api helper
                string apiKey = _apiHelper.ApiKey;

                //integrate api endpoint 
                string endpoint = $"group?id={cityId}&units=metric&appid={apiKey}";

                

                //check if weatherdata key available
                if (_cacheServices.Get<List<WeatherDetail>>("WeatherData") is null)
                {

                    //call getasync method in apihelper class
                    //returns json data or throw exception
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

                          //retrieve weatherdata values    

                weatherList = _cacheServices.Get<List<WeatherDetail>>("WeatherData");

                              
                

                return weatherList;

            }
            catch (ApiException ex)
            {
                // Handle api exceptions 
                if (ex.StatusCode == 404)
                {
                    return "NotFound";
                }
                else if (ex.StatusCode == 401)
                {
                    return "Unauthorized";
                }
                else if(ex.StatusCode == 403)
                {
                    return "Forbidden";
                }
                else if(ex.StatusCode == 400)
                {
                    return "BadRequest";
                }
                else if (ex.StatusCode == 500)
                {
                    return "ServerError";
                }
                else
                {
                    return "UnknownError";
                }
            }
            catch(JsonSerializationException)
            {
                //handle any form of serialization exceptions
                throw new Exception("SerializationError");
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
