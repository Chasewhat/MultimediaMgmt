using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaMgmt.Common.Helper
{
    public class DateTimeHelper
    {
        public static int GetWeekOfYear(DateTime dt)
        {
            return new GregorianCalendar().GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }
}
