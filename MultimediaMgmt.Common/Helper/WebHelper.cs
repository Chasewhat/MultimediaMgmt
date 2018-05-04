using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MultimediaMgmt.Common.Helper
{
    /// <summary>
    /// Web请求帮助类
    /// </summary>
    public class WebHelper
    {
        /// <summary>
        /// 向指定url发送请求,并接收返回值
        /// </summary>
        /// <param name="url">web请求地址</param>
        /// <param name="encoding">编码类型</param>
        /// <param name="timeout">超时限制</param>
        /// <returns></returns>
        public static string Get(string url, Encoding encoding,
            int timeout = 5000)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.Timeout = timeout;
            HttpWebResponse res = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream(), encoding);
            string html = sr.ReadToEnd();
            sr.Close();
            res.Close();
            return html;
        }
        /// <summary>
        /// 默认编码请求
        /// </summary>
        public static string Get(string url, int timeout = 5000)
        {
            return Get(url, Encoding.Default, timeout);
        }
        /// <summary>
        /// 向指定url发送请求,并读取返回数据流写入本地文件
        /// </summary>
        /// <param name="url">web请求地址</param>
        /// <param name="path">文件保存路径</param>
        /// <param name="timeout">超时限制</param>
        /// <returns></returns>
        public static bool DownloadFile(string url, string path, int timeout = 3000)
        {
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Timeout = timeout;

                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();

                //创建本地文件写入流
                Stream stream = new FileStream(path, FileMode.Create);

                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                responseStream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}