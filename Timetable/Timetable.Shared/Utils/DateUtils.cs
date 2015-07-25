using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Utils
{
    public class DateUtils
    {
        private const string DATE_FORMAT = "dd.MM.yyyy";
        private static readonly CultureInfo CULTURE = new CultureInfo("ru-RU");

        private DateUtils() { }

        public static DateTime ToDate(string value)
        {
            return DateTime.ParseExact(value, DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        public static bool IsDate(string value)
        {
            try
            {
                DateTime.ParseExact(value, DATE_FORMAT, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }

        public static string GetDayOfTheWeek(string date)
        {
            return GetDayOfTheWeek(ToDate(date));
        }

        public static string GetDayOfTheWeek(DateTime date)
        {
            return date.ToString("dddd", CULTURE);
        }
    }
}
