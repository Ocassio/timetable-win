using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using Newtonsoft.Json;
using Timetable.Models;
using Timetable.Utils;

namespace Timetable.Providers
{
    public class SettingsProvider
    {
        private const string GROUP_KEY = "group";
        private const string DATE_RANGE_TYPE_KEY = "dateRangeType";
        private const string CUSTOM_DATE_RANGE_KEY = "customDateRange";

        private const string DEFAULT_DATE_TYPE = "currentWeek";

        private SettingsProvider() { }

        public static string DateRangeType
        {
            set
            {
                ApplicationData.Current.LocalSettings.Values[DATE_RANGE_TYPE_KEY] = value;
            }
            get
            {
                var type = (string) ApplicationData.Current.LocalSettings.Values[DATE_RANGE_TYPE_KEY];
                return type ?? DEFAULT_DATE_TYPE;
            }
        }

        public static DateRange CustomDateRange
        {
            set
            {
                ApplicationData.Current.LocalSettings.Values[CUSTOM_DATE_RANGE_KEY] = JsonConvert.SerializeObject(value);
            }
            get
            {
                var dateRange = (string) ApplicationData.Current.LocalSettings.Values[CUSTOM_DATE_RANGE_KEY];
                return dateRange != null ? JsonConvert.DeserializeObject<DateRange>(dateRange) : DateUtils.GetDefaultDateRange();
            }
        }

        public static Group Group
        {
            set
            {
                ApplicationData.Current.LocalSettings.Values[GROUP_KEY] = JsonConvert.SerializeObject(value);
            }
            get
            {
                var group = (string) ApplicationData.Current.LocalSettings.Values[GROUP_KEY];
                return group != null ? JsonConvert.DeserializeObject<Group>(group) : null;
            }
        }
    }
}
