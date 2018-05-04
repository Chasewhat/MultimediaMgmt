using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MultimediaMgmt.Common.Config
{
    /// <summary>
    /// 配置基础类,提供配置文件的读写通用功能
    /// 每个配置的实例对象对应一个配置文件
    /// </summary>
    public class ConfigBase
    {
        /// <summary>
        /// 控件配置文件修改之后是否自动保存,默认为true
        /// </summary>
        public bool AutoSave = true;
        //对应的配置文件路径
        private string ConFilePath;
        //配置信息存储字典
        public Dictionary<string, string> CONFIGS = new Dictionary<string, string>();

        #region Read
        /// <summary>
        /// 配置文件初始化,读取所有配置信息
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        public virtual void Init(string configFilePath)
        {
            if (File.Exists(configFilePath))
            {
                ConFilePath = configFilePath;
                Read();
            }
        }
        /// <summary>
        /// 重新读取加载配置信息
        /// </summary>
        public void Reload()
        {
            if (File.Exists(ConFilePath))
            {
                CONFIGS.Clear();
                Read();
            }
        }

        /// <summary>  
        /// 读取配置文件的属性值  
        /// </summary>  
        private void Read()
        {
            StreamReader sr = new StreamReader(ConFilePath, Encoding.UTF8);
            int spidx = -1;
            string line, key, value;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                spidx = line.IndexOf('=');
                if (spidx > 0)
                {
                    key = line.Substring(0, spidx).ToUpper();
                    value = line.Substring(spidx + 1);
                    if (!CONFIGS.ContainsKey(key))
                        CONFIGS.Add(key, value);
                }
            }
            sr.Close();
        }

        /// <summary>  
        /// 返回变量的字符串值  
        /// </summary>  
        /// <param name="cName">变量名称</param>  
        /// <returns>变量值</returns>  
        public string Get(string cName)
        {
            cName = cName.ToUpper();
            if (CONFIGS.ContainsKey(cName))
                return CONFIGS[cName];
            else
                return null;
        }

        /// <summary>  
        /// 返回变量的int值  
        /// </summary>  
        /// <param name="cName">变量名称</param>  
        /// <returns>变量值</returns>  
        public int GetInt(string cName)
        {
            return (int)GetDouble(cName);
        }

        /// <summary>  
        /// 返回变量的double值  
        /// </summary>  
        /// <param name="cName">变量名称</param>
        /// <returns>变量值</returns>  
        public double GetDouble(string cName)
        {
            double val = 0;
            double.TryParse(Get(cName), out val);
            return val;
        }
        #endregion

        #region Write
        /// <summary>  
        /// 设置写入配置文件 
        /// </summary>  
        /// <param name="cName">属性名称</param>  
        /// <param name="cValue">值</param>  
        public void Set(string cName, string cValue)
        {
            cName = cName.ToUpper();
            if (CONFIGS.ContainsKey(cName))
                CONFIGS[cName] = cValue;
            else
                CONFIGS.Add(cName, cValue);
            if (AutoSave)
                Save();
        }
        /// <summary>
        /// 批量设置写入配置文件
        /// </summary>
        /// <param name="cNameValues">键值对集合</param>
        public void SetRange(List<KeyValuePair<string, string>> cNameValues)
        {
            string temp = string.Empty;
            cNameValues.ForEach(s =>
            {
                temp = s.Key.ToUpper();
                if (CONFIGS.ContainsKey(temp))
                    CONFIGS[temp] = s.Value;
                else
                    CONFIGS.Add(temp, s.Value);
            });
            if (AutoSave)
                Save();
        }

        /// <summary>  
        /// 将设置的值写入到ini文件中.
        /// </summary>
        /// <returns></returns>
        public void Save()
        {
            try
            {
                StreamWriter sw = new StreamWriter(ConFilePath, false, Encoding.UTF8);
                lock (CONFIGS)
                {
                    foreach (var kvp in CONFIGS)
                    {
                        sw.WriteLine("{0}={1}", kvp.Key, kvp.Value);
                    }
                }
                sw.Close();
            }
            catch { }
        }
        #endregion
    }
}