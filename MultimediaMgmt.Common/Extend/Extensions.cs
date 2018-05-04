using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;

namespace MultimediaMgmt.Common.Extend
{
    /// <summary>
    /// 扩展方法支持类
    /// </summary>
    public static class Extensions
    {
        #region IEnumerable
        /// <summary>
        /// 将泛型IEnumerable转换为泛型ObservableCollection
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">this基于IEnumerable的方法扩展</param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("error");
            }
            return new ObservableCollection<T>(source);
        }
        /// <summary>
        /// 将泛型IEnumerable转换为泛型SmartObservableCollection
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">this基于IEnumerable的方法扩展</param>
        /// <returns></returns>
        public static SmartObservableCollection<T> ToSmartObservableCollection<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("error");
            }
            return new SmartObservableCollection<T>(source);
        }
        #endregion

        #region String
        /// <summary>
        /// 判断字符串是否可以转换为int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            int i;
            return int.TryParse(str, out i);
        }

        /// <summary>
        /// 将字符串转换为int,转换失败返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            if (str.IsInt())
                return int.Parse(str);
            else
                return 0;
        }
        /// <summary>
        /// 判断字符串是否可以转换为double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDouble(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            double i;
            return double.TryParse(str, out i);
        }

        /// <summary>
        /// 将字符串转换为double,转换失败返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            if (str.IsDouble())
                return double.Parse(str);
            else
                return 0;
        }
        /// <summary>
        /// 判断字符串是否匹配指定正则表达式
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern">正则表达式</param>
        /// <returns></returns>
        public static bool IsMatch(this string str, string pattern)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(pattern))
                return false;
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 将指定运算表达式字符串执行简单运算,并返回指定位数的四舍五入结果
        /// </summary>
        /// <param name="str"></param>
        /// <param name="round">四舍五入位数</param>
        /// <returns></returns>
        public static double ExecCompute(this string str, int round = 2)
        {
            double result = 0;
            try
            {
                if (double.TryParse(new DataTable().Compute(str, "").ToString(), out result))
                {
                    //排除无穷大小及非数字的情况
                    if (double.IsInfinity(result) || double.IsNaN(result))
                        result = 0;
                    else
                        result = Math.Round(result, round);
                }
                else
                {
                    result = 0;
                }
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 判断字符串是否为datetime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            DateTime result;
            return DateTime.TryParse(str, out result);
        }
        /// <summary>
        /// 将字符串转换为可null DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ToNullableDateTime(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            DateTime result;

            if (DateTime.TryParse(str, out result))
                return result;

            return null;
        }
        /// <summary>
        /// 将字符串转换为DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            if (str.IsDateTime())
                return DateTime.Parse(str);
            return DateTime.MinValue;
        }
        /// <summary>
        /// 判断字符串是否为指定格式的datetime
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format">日期格式</param>
        /// <returns></returns>
        public static bool IsDateTime(this string str, string format = "yyyy-MM-dd")
        {
            if (string.IsNullOrEmpty(str))
                return false;
            DateTime result;
            return DateTime.TryParseExact(str, format, null, System.Globalization.DateTimeStyles.None, out result);
        }
        /// <summary>
        /// 将字符串转换为指定格式的可null DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format">日期格式</param>
        /// <returns></returns>
        public static DateTime? ToNullableDateTime(this string str, string format = "yyyy-MM-dd")
        {
            if (string.IsNullOrEmpty(str))
                return null;
            DateTime result;

            if (DateTime.TryParseExact(str, format, null, System.Globalization.DateTimeStyles.None, out result))
                return result;

            return null;
        }
        /// <summary>
        /// 将字符串转换为指定格式的DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str, string format)
        {
            if (str.IsDateTime(format))
                return DateTime.ParseExact(str, format, null);
            return DateTime.MinValue;
        }

        public static string ToTimeString(this string str, string format = "HH:mm:ss")
        {
            DateTime dt;
            if (DateTime.TryParse(str, out dt))
                return dt.ToString(format);
            else
                return str;
        }
        #endregion

        #region Int
        /// <summary>
        /// 将Int转换为指定格式的DateTime
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToDate(this int dt)
        {
            return new DateTime((dt / 10000), (dt % 10000 / 100), (dt % 100));
        }

        /// <summary>
        /// 将Int转换为指定格式的DateTime
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(int date, int time)
        {
            if (date == 0) return DateTime.Now.Date.AddHours(time / 10000).AddMinutes(time % 10000 / 100).AddSeconds(time % 100);
            else return new DateTime((date / 10000), (date % 10000 / 100), (date % 100), (time / 10000), (time % 10000 / 100), (time % 100));
        }

        public static DateTime ToDateTime(this int date)
        {
            return new DateTime((date / 10000), (date % 10000 / 100), (date % 100));
        }
        #endregion

        #region DateTime
        /// <summary>
        /// 将日期转换为指定格式的int
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format">转换格式</param>
        /// <returns></returns>
        public static int ToInt(this DateTime dt, string format = "HHmmss")
        {
            return dt.ToString(format).ToInt();
        }
        #endregion

        #region Enum
        public static string GetDescription(this Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }

        public static Dictionary<int, string> ToDictionary(this Enum sourceEnum, int max = 0)
        {
            Dictionary<int, string> dicEnum = new Dictionary<int, string>();
            Type enumType = sourceEnum.GetType();
            string[] fieldstrs = Enum.GetNames(enumType); //获取枚举字段数组  
            string description = string.Empty;
            int i = 0;
            foreach (var item in fieldstrs)
            {
                i++;
                if (max > 0 && i > max)
                    break;
                var field = enumType.GetField(item);
                object[] arr = field.GetCustomAttributes(typeof(DescriptionAttribute), true);//获取属性字段数组  
                if (arr != null && arr.Length > 0)
                    description = ((DescriptionAttribute)arr[0]).Description;//属性描述  
                else
                    description = item;//描述不存在取字段名称  
                dicEnum.Add((int)Enum.Parse(enumType, item), description);
            }
            return dicEnum;
        }
        #endregion
    }
}
