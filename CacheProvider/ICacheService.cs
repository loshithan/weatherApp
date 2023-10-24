using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weatherApp
{
    public interface ICacheService
    {
        object Get(string key);
        void Insert(string key, object value, DateTime absoluteExpiration);
        void Remove(string key);
    }
}
