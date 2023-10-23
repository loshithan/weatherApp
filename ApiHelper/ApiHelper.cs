using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using weatherApp.DAL;
using Newtonsoft.Json;
using System.Net;
using weatherApp.CustomException;
using System.Web.Mvc;

namespace weatherApp.DAL
{
    public class ApiHelper:IApiHelper
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;
        private readonly string _apiKey;

        public ApiHelper(HttpClient httpClient)
        {
            //retrieve apikey from webconfig file 
            _apiKey = ConfigurationManager.AppSettings["ApiKey"];

            //retrive baseurl from constants file
            _baseApiUrl = Constants.BaseUrl;

            //instantiate httpclient
            _httpClient = httpClient;
        }

        public string ApiKey { get { return _apiKey; } }


        public async Task<string> GetAsync(string endpoint)
        {
            string apiUrl = $"{_baseApiUrl}/{endpoint}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
               

                //if success code 200 return json result
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return json;
                }
                
                else 
                {
                    //throw custom exception for status code except 200
                    throw new ApiException((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                }

            }
            //catch above thrown 
            catch (HttpRequestException ex)
            {
                // Handle HTTP request exceptions here
                throw new ApiException(500,ex.Message);
/*                return ex.Message;
*/            }
        } 
    
    

    }
}