using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using weatherApp.DAL;
using Newtonsoft.Json;



namespace weatherApp.DAL
{
    public class ApiHelper
    {
        
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;
        private readonly string _apiKey;

        public ApiHelper()
        {
            _apiKey = ConfigurationManager.AppSettings["ApiKey"];

            _baseApiUrl = Constants.BaseUrl;
            _httpClient = new HttpClient();
        }

        public string ApiKey { get { return _apiKey; } }

        public async Task<string> GetAsync(string endpoint)
        {
            string apiUrl = $"{_baseApiUrl}/{endpoint}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return json;
                }
                else
                {
                    // Handle non-success status codes here
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                // Handle HTTP request exceptions here
                return null;
            }
        }
    

    }
}