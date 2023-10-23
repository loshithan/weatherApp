using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weatherApp.Models;

namespace weatherApp.DAL
{
    //interface to access implementations of dataaccess layer
    public interface IDataAccessLayer
    {
        Task<object> GetWeatherData();
        List<City> ReadJsonFromFile();
        WeatherDetail GetWeatherDetailAsync(int? code);
    }
}
