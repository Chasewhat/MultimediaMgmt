using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Text;

namespace Common.Helper
{
    /// <summary>
    /// 文件/文件夹处理帮助类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="srcdir">源文件夹</param>
        /// <param name="desdir">目标文件夹</param>
        public static void CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);
            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }
                    CopyDirectory(file, desfolderdir);
                }

                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);
                    srcfileName = desfolderdir + "\\" + srcfileName;
                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }
                    File.Copy(file, srcfileName, true);
                }
            }
        }
        /// <summary>
        /// 文件夹压缩
        /// </summary>
        /// <param name="strDir">压缩源文件夹</param>
        /// <param name="strZip">压缩文件名(zip)</param>
        public static void Zip(string strDir, string strZip)
        {
            FastZip fastZip = new FastZip();
            fastZip.CreateZip(strZip, strDir, true, null);
        }
        /// <summary>
        /// 文件夹解压
        /// </summary>
        /// <param name="strDir">解压目标文件夹</param>
        /// <param name="strZip">压缩文件名(zip)</param>
        public static void UnZip(string strDir, string strZip)
        {
            FastZip fastZip = new FastZip();
            fastZip.ExtractZip(strZip, strDir, null);
        }

        /// <summary>
        /// 将文本写入文件
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="fileName">文件全路径</param>
        /// <param name="fileMode">写入方式</param>
        public static void WriteStringToFile(string content, string fileName, FileMode fileMode)
        {
            FileStream fs = new FileStream(fileName, fileMode, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write(content);
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 从文件读取文本
        /// </summary>
        /// <param name="fileName"></param>
        public static string ReadStringFromFile(string fileName)
        {
            if (!File.Exists(fileName))
                return string.Empty;
            StreamReader sw = new StreamReader(fileName, Encoding.Default);
            string result = sw.ReadToEnd();
            return result;
        }

    }
}