using System;
using System.IO;

namespace MultimediaMgmt.Common.Helper
{
    /// <summary>
    /// 日志记录帮助类,记录为.log文本文档
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 日志存储路径--[程序主目录/Log/yyyyMMdd.log],每天记录一个日志文件
        /// </summary>
        private static string path
        {
            get
            {
                return string.Format("{0}\\Log\\{1}.log", Environment.CurrentDirectory, DateTime.Now.ToString("yyyyMMdd"));
            }
        }
        /// <summary>
        /// 记录指定日志信息
        /// </summary>
        /// <param name="info">日志信息字符串</param>
        public static void Write(string info)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Seek(0, SeekOrigin.End);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.WriteLine(string.Format("{0} {1}", DateTime.Now, info));
                sw.WriteLine();
                sw.Close();
                fs.Close();
            }
            catch { }
        }
        /// <summary>
        /// 记录日志信息及异常信息
        /// </summary>
        /// <param name="info">日志信息</param>
        /// <param name="ex">异常对象信息</param>
        public static void Write(string info, Exception ex)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Seek(0, SeekOrigin.End);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.WriteLine(string.Format("{0} {1}", DateTime.Now, info));
                sw.WriteLine(string.Format("{0} {1}", DateTime.Now, ex));
                sw.WriteLine();
                sw.Close();
                fs.Close();
            }
            catch { }
        }
    }
}