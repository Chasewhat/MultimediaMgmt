using MultimediaMgmt.Common.Config;
using System;

namespace MultimediaMgmt.Common.Helper
{
    /// <summary>
    /// 配置帮助类,执行所有配置文件的初始化及静态访问
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 系统主配置
        /// </summary>
        public static MainConfig Main = new MainConfig();

        /// <summary>
        /// 配置初始化
        /// </summary>
        public static void Init()
        {
            Main.Init(Environment.CurrentDirectory + "\\Config\\Main.ini");
        }
    }
}