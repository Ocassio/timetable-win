using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Models;
using Timetable.Providers;

namespace Timetable.Utils
{
    public class DateUtils
    {
        private const string DATE_FORMAT = "dd.MM.yyyy";
        private static readonly CultureInfo CULTURE = new CultureInfo("ru-RU");

        private static readonly Dictionary<string, Func<DateRange>> DATE_RANGES_MAP = new Dictionary<string, Func<DateRange>> 
        {
            { "sevenDays", GetSevenDaysRange },
            { "currentWeek", GetCurrentWeekRange },
            { "nextWeek", GetNextWeekRange },
            { "currentMonth", GetCurrentMonthRange }
        };

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

        public static string ToString(DateTime value)
        {
            return value.ToString(DATE_FORMAT);
        }

        public static string GetDayOfTheWeek(string date)
        {
            return GetDayOfTheWeek(ToDate(date));
        }

        public static string GetDayOfTheWeek(DateTime date)
        {
            return date.ToString("dddd", CULTURE);
        }

        public static DateRange GetDefaultDateRange()
        {
            return new DateRange(DateTime.Today, DateTime.Today);
        }

        public static DateRange GetSevenDaysRange()
        {
            var from = DateTime.Today;
            var to = from.AddDays(6);

            return new DateRange(from, to);
        }

        public static DateRange GetCurrentWeekRange()
        {
            var from = DateTime.Today.AddDays(-1 * (int) DateTime.Today.DayOfWeek + 1);
            var to = from.AddDays(6);

            return new DateRange(from, to);
        }

        public static DateRange GetNextWeekRange()
        {
            var from = DateTime.Today.AddDays(-1 * (int) DateTime.Today.DayOfWeek + 8);
            var to = from.AddDays(6);

            return new DateRange(from, to);
        }

        public static DateRange GetCurrentMonthRange()
        {
            var from = DateTime.Today.AddDays(-1 * DateTime.Today.Day + 1);
            var to = from.AddDays(DateTime.DaysInMonth(from.Year, from.Month) - 1);

            return new DateRange(from, to);
        }

        public static DateRange GetCustomDateRange()
        {
            return SettingsProvider.CustomDateRange;
        }

        public static DateRange GetDateRange(string key)
        {
            return DATE_RANGES_MAP[key]();
        }
    }
}
