using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaMgmt.Common.Helper
{
    /// <summary>
    /// 基于Newtonsoft.Json的json处理帮助类
    /// </summary>
    public class JsonHelper
    {
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                StringReader sr = new StringReader(json);
                return serializer.Deserialize(new JsonTextReader(sr), typeof(T)) as T;
            }
            catch
            {
                return null;
            }
        }

        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                StringReader sr = new StringReader(json);
                return serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>)) as List<T>;
            }
            catch
            {
                return null;
            }
        }

        public static string SerializeObjectToJson(object obj)
        {
            JsonSerializer serializer = new JsonSerializer();
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            serializer.Serialize(new JsonTextWriter(sw), obj);
            return sb.ToString();
        }
    }
}