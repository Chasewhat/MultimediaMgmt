using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MultimediaMgmt.ViewModel
{
    public class RestConnection : IRestConnection
    {
        private RestClient client;
        private string __RequestVerificationToken;
        public RestConnection(string baseUrl)
        {
            client = new RestClient(baseUrl);
        }
        private string GetRequestVerificationToken(string content)
        {
            string result = "";
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//input[@name = '__RequestVerificationToken' and @type = 'hidden']");
            result = node?.GetAttributeValue("value", "");
            return result;
        }
        private string GetLogoffVerificationToken(string content)
        {
            string result = "";
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            doc.LoadHtml(doc.DocumentNode.SelectSingleNode("//script[@id = 'logintmpl']")?.InnerHtml);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//input[@name = '__RequestVerificationToken' and @type = 'hidden']");
            result = node?.GetAttributeValue("value", "");
            return result;
        }
        private string GetLoginErrorMsg(string content)
        {
            string result = "";
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@class = 'validation-summary-errors msg-error']/ul/li");
            result = node.InnerText;
            return result;
        }
        public bool Login(string url, string usernameKey, string username, string passwordKey, string password, out string msg)
        {
            msg = "";
            client.CookieContainer = null;
            var loginRequest = new RestRequest(url, Method.GET);
            IRestResponse loginResponse = client.Execute(loginRequest);
            if (loginResponse.StatusCode != HttpStatusCode.OK)
            {
                msg = loginResponse.ErrorMessage;
                if (string.IsNullOrEmpty(msg))
                {
                    msg = loginResponse.StatusDescription;
                }
                return false;
            }
            string sVerificationToken = GetRequestVerificationToken(loginResponse.Content);
            if (string.IsNullOrEmpty(sVerificationToken)) return false;
            CookieContainer cookieContainer = new CookieContainer();
            foreach (var cookie in loginResponse.Cookies)
            {
                cookieContainer.Add(new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
            }
            var login = new RestRequest(url, Method.POST);
            //login.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //login.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            //login.AddHeader("Accept-Encoding", "gzip, deflate, br");
            //login.AddHeader("Connection", "keep-alive");
            client.CookieContainer = cookieContainer;
            //client.Authenticator = new SimpleAuthenticator(usernameKey, username, passwordKey, password);
            //login.AddCookie("__RequestVerificationToken", __RequestVerificationToken);
            login.AddParameter("__RequestVerificationToken", sVerificationToken);
            login.AddParameter(usernameKey, username);
            login.AddParameter(passwordKey, password);
            loginResponse = client.Execute(login);
            if (loginResponse.StatusCode != HttpStatusCode.OK)
            {
                msg = loginResponse.ErrorMessage;
                if (string.IsNullOrEmpty(msg))
                {
                    msg = loginResponse.StatusDescription;
                }
                return false;
            }
            if (loginResponse.ResponseUri.AbsolutePath.Equals(@"/Account/Login"))
            {
                msg = GetLoginErrorMsg(loginResponse.Content);
                return false;
            }
            __RequestVerificationToken = GetLogoffVerificationToken(loginResponse.Content);
            return true;
        }
        public bool Logout(string url)
        {
            var logoff = new RestRequest(url, Method.POST);
            logoff.AddParameter("__RequestVerificationToken", __RequestVerificationToken);
            var logoffResponse = client.Execute(logoff);
            if (logoffResponse.StatusCode != HttpStatusCode.OK) return false;
            client.Authenticator = null;
            client.CookieContainer = null;
            return true;
        }

        public JObject Get(string url, Dictionary<string, dynamic> parameters)
        {
            JObject result = null;
            var request = new RestRequest(url, Method.GET);
            if (null != parameters)
            {
                foreach (var parameter in parameters)
                {
                    request.AddParameter(parameter.Key, parameter.Value);
                }
            }
            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw (new Exception(response.StatusDescription));
            }
            try
            {
                result = JObject.Parse(response.Content);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public JObject Get(string url, Dictionary<string, string> parameters)
        {
            JObject result = null;
            var request = new RestRequest(url, Method.GET);
            if (null != parameters)
            {
                foreach (var parameter in parameters)
                {
                    request.AddParameter(parameter.Key, parameter.Value);
                }
            }
            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw (new Exception(response.StatusDescription));
            }
            try
            {
                result = JsonConvert.DeserializeObject(response.Content) as JObject;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public JObject Post(string url, object body)
        {
            JObject result = null;
            var request = new RestRequest(url, Method.POST);
            //request.RequestFormat = DataFormat.Json;
            //request.AddBody(body);
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw (new Exception(response.StatusDescription));
            }
            try
            {
                result = JObject.Parse(response.Content);
                if (!result.Value<bool>("success"))
                {
                    throw new Exception(result.Value<string>("message"));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        protected T DeserializeJson<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            return serializer.Deserialize(new JsonTextReader(sr), typeof(T)) as T;
        }

        public Collection<T> GetPageValues<T>(string url, int page, int start, int limit, Dictionary<string, string> parameters, ref int total)
        {
            Collection<T> result = null;
            if (null == parameters)
            {
                parameters = new Dictionary<string, string>();
            }
            parameters.Add("_dc", "1504179824079");
            parameters.Add("page", page.ToString());
            parameters.Add("start", start.ToString());
            parameters.Add("limit", limit.ToString());
            parameters.Add("sWhere", "");
            JObject jo = Get(url, parameters);
            if (jo.Value<bool>("success"))
            {
                total = jo.Value<int>("total");
                JArray ja = jo.Value<JArray>("data");
                if (null != ja)
                {
                    result = ja.ToObject<Collection<T>>();
                }
            }
            else
            {
                throw (new Exception(jo.Value<string>("message")));
            }
            return result;
        }
    }
}
