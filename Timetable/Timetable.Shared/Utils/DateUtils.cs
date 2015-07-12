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

        private DateUtils() { }

        public static bool IsDate(string value)
        {
            try
            {
                DateTime.ParseExact(value, DATE_FORMAT, CultureInfo.InvariantCulture);
            }
            catch (FormatException e)
            {
                return false;
            }

            return true;
        }
    }
}
