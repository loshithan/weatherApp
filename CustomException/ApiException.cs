using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace weatherApp.CustomException
{
    public class ApiException:Exception
    {
        public int StatusCode { get; }
        public string Content { get; }

        public ApiException(int statusCode, string content):base(content)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}