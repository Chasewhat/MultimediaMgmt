using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MultimediaMgmt.ViewModel
{
    public interface IRestConnection
    {
        bool Login(string url, string usernameKey, string username, string passwordKey, string password, out string msg);
        bool Logout(string url);
        JObject Get(string url, Dictionary<string, string> parameters);
        JObject Post(string url, object body);
        Collection<T> GetPageValues<T>(string url, int page, int start, int limit, Dictionary<string, string> parameters, ref int total);
    }
}
