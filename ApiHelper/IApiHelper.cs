using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weatherApp.DAL
{
    //interface to access api
    public interface IApiHelper
    {
        string ApiKey { get; }

        Task<string> GetAsync(string endpoint);
    }
}
