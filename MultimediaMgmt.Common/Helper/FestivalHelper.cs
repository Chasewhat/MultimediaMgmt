using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class FestivalHelper
    {
        private static ChineseLunisolarCalendar china = new ChineseLunisolarCalendar();
        private static Dictionary<int, string> gHoliday = new Dictionary<int, string>();
        private static Dictionary<int, string> nHoliday = new Dictionary<int, string>();
        private static string[] JQ = { "小寒", "大寒", "立春", "雨水", "惊蛰", "春分", "清明", "谷雨", "立夏", "小满", "芒种", "夏至", "小暑", "大暑", "立秋", "处暑", "白露", "秋分", "寒露", "霜降", "立冬", "小雪", "大雪", "冬至" };
        private static int[] JQData = { 0, 21208, 43467, 63836, 85337, 107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758 };

        static FestivalHelper()
        {
            //公历节日
            gHoliday.Add(101, "元旦");
            gHoliday.Add(214, "情人节");
            gHoliday.Add(305, "雷锋日");
            gHoliday.Add(308, "妇女节");
            gHoliday.Add(312, "植树节");
            gHoliday.Add(315, "消费者权益日");
            gHoliday.Add(401, "愚人节");
            gHoliday.Add(501, "劳动节");
            gHoliday.Add(504, "青年节");
            gHoliday.Add(601, "儿童节");
            gHoliday.Add(701, "建党节");
            gHoliday.Add(801, "建军节");
            gHoliday.Add(910, "教师节");
            gHoliday.Add(1001, "国庆节");
            gHoliday.Add(1224, "平安夜");
            gHoliday.Add(1225, "圣诞节");

            //农历节日
            nHoliday.Add(101, "春节");
            nHoliday.Add(115, "元宵节");
            nHoliday.Add(505, "端午节");
            nHoliday.Add(815, "中秋节");
            nHoliday.Add(909, "重阳节");
            nHoliday.Add(1208, "腊八节");
        }

        /// <summary>
        /// 获取农历
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetChinaDate(DateTime dt)
        {
            if (dt > china.MaxSupportedDateTime || dt < china.MinSupportedDateTime)
            {
                //日期范围：1901 年 2 月 19 日 - 2101 年 1 月 28 日
                throw new Exception(string.Format("日期超出范围！必须在{0}到{1}之间！", china.MinSupportedDateTime.ToString("yyyy-MM-dd"), china.MaxSupportedDateTime.ToString("yyyy-MM-dd")));
            }
            string str = string.Format("{0} {1}{2}", GetYear(dt), GetMonth(dt), GetDay(dt));
            string strJQ = GetSolarTerm(dt);
            if (strJQ != "")
            {
                str += " (" + strJQ + ")";
            }
            string strHoliday = GetHoliday(dt);
            if (strHoliday != "")
            {
                str += " " + strHoliday;
            }
            string strChinaHoliday = GetChinaHoliday(dt);
            if (strChinaHoliday != "")
            {
                str += " " + strChinaHoliday;
            }

            return str;
        }

        /// <summary>
        /// 获取农历年份
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetYear(DateTime dt)
        {
            int yearIndex = china.GetSexagenaryYear(dt);
            string yearTG = " 甲乙丙丁戊己庚辛壬癸";
            string yearDZ = " 子丑寅卯辰巳午未申酉戌亥";
            string yearSX = " 鼠牛虎兔龙蛇马羊猴鸡狗猪";
            int year = china.GetYear(dt);
            int yTG = china.GetCelestialStem(yearIndex);
            int yDZ = china.GetTerrestrialBranch(yearIndex);

            string str = string.Format("[{1}]{2}{3}{0}", year, yearSX[yDZ], yearTG[yTG], yearDZ[yDZ]);
            return str;
        }

        /// <summary>
        /// 获取农历月份
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetMonth(DateTime dt)
        {
            int year = china.GetYear(dt);
            int iMonth = china.GetMonth(dt);
            int leapMonth = china.GetLeapMonth(year);
            bool isLeapMonth = iMonth == leapMonth;
            if (leapMonth != 0 && iMonth >= leapMonth)
            {
                iMonth--;
            }

            string szText = "正二三四五六七八九十";
            string strMonth = isLeapMonth ? "闰" : "";
            if (iMonth <= 10)
            {
                strMonth += szText.Substring(iMonth - 1, 1);
            }
            else if (iMonth == 11)
            {
                strMonth += "十一";
            }
            else
            {
                strMonth += "腊";
            }
            return strMonth + "月";
        }

        /// <summary>
        /// 获取农历日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetDay(DateTime dt)
        {
            int iDay = china.GetDayOfMonth(dt);
            string szText1 = "初十廿三";
            string szText2 = "一二三四五六七八九十";
            string strDay;
            if (iDay == 20)
            {
                strDay = "二十";
            }
            else if (iDay == 30)
            {
                strDay = "三十";
            }
            else
            {
                strDay = szText1.Substring((iDay - 1) / 10, 1);
                strDay = strDay + szText2.Substring((iDay - 1) % 10, 1);
            }
            return strDay;
        }

        /// <summary>
        /// 获取节气
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetSolarTerm(DateTime dt)
        {
            DateTime dtBase = new DateTime(1900, 1, 6, 2, 5, 0);
            DateTime dtNew;
            double num;
            int y;
            string strReturn = "";

            y = dt.Year;
            for (int i = 1; i <= 24; i++)
            {
                num = 525948.76 * (y - 1900) + JQData[i - 1];
                dtNew = dtBase.AddMinutes(num);
                if (dtNew.DayOfYear == dt.DayOfYear)
                {
                    strReturn = JQ[i - 1];
                }
            }

            return strReturn;
        }

        /// <summary>
        /// 获取公历节日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetHoliday(DateTime dt)
        {
            string strReturn = "";
            string g = gHoliday[dt.Month * 100 + dt.Day];
            if (g != "")
            {
                strReturn = g;
            }

            return strReturn;
        }

        public static int GetHolidayDateByName(string name)
        {
            return gHoliday.FirstOrDefault(o => o.Value == name).Key;
        }

        public static int GetChinaHolidayDateByName(string name)
        {
            return nHoliday.FirstOrDefault(o => o.Value == name).Key;
        }

        /// <summary>
        /// 获取农历节日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetChinaHoliday(DateTime dt)
        {
            string strReturn = "";
            int year = china.GetYear(dt);
            int iMonth = china.GetMonth(dt);
            int leapMonth = china.GetLeapMonth(year);
            int iDay = china.GetDayOfMonth(dt);
            if (china.GetDayOfYear(dt) == china.GetDaysInYear(year))
            {
                strReturn = "除夕";
            }
            else if (leapMonth != iMonth)
            {
                if (leapMonth != 0 && iMonth >= leapMonth)
                {
                    iMonth--;
                }
                string n = nHoliday[iMonth * 100 + iDay];
                if (n != "")
                {
                    if (strReturn == "")
                    {
                        strReturn = n;
                    }
                    else
                    {
                        strReturn += " " + n;
                    }
                }
            }

            return strReturn;
        }
        #region 阴历-阳历-转换

        /// <summary>  
        /// 阴历转为阳历  
        /// </summary>  
        /// <param name="year">指定的年份</param>  
        public static DateTime GetLunarYearDate(DateTime dt)
        {
            int cnYear = china.GetYear(dt);
            int cnMonth = china.GetMonth(dt);
            int num1 = 0;
            int num2 = china.IsLeapYear(cnYear) ? 13 : 12;
            while (num2 >= cnMonth)
            {
                num1 += china.GetDaysInMonth(cnYear, num2--);
            }
            num1 = num1 - china.GetDayOfMonth(dt) + 1;
            return dt.AddDays(num1);
        }

        public static DateTime GetLunarDate(DateTime solar)
        {
            int year = solar.Year;
            int month = solar.Month;
            int day = Math.Min(solar.Day, china.GetDaysInMonth(year, month));
            int leapMonth = china.GetLeapMonth(year);
            if (0 < leapMonth && leapMonth <= month)
                ++month;
            return china.ToDateTime(year, month, day, 0, 0, 0, 0);
        }

        /// <summary>  
        /// 阳历转为阴历  
        /// </summary>  
        /// <param name="dt">公历日期</param>  
        /// <returns>农历的日期</returns>
        public static DateTime GetSunYearDate(DateTime dt)
        {
            int year = china.GetYear(dt);
            int iMonth = china.GetMonth(dt);
            int iDay = china.GetDayOfMonth(dt);
            int leapMonth = china.GetLeapMonth(year);
            bool isLeapMonth = iMonth == leapMonth;
            if (leapMonth != 0 && iMonth >= leapMonth)
                iMonth--;
            string str = string.Format("{0}-{1}-{2}", year, iMonth, iDay);
            DateTime dtNew = DateTime.Now;
            try
            {
                dtNew = Convert.ToDateTime(str); //防止出现2月份时，会出现超过时间，出现“2015-02-30”这种错误日期  
            }
            catch{}
            return dtNew;
        }

        #endregion  
    }
}
