using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Models;
using Timetable.Utils;

namespace Timetable.Providers
{
    public class DataProvider
    {
        private const string ATTR_VALUE = "value";

        private const string REL_GROUP = "0";

        private const int COL_COUNT = 7;

        private const string TIMETABLE_URL = "http://www.tolgas.ru/services/raspisanie/";

        public async static Task<ObservableCollection<Group>> GetGroups()
        {
            var groups = new ObservableCollection<Group>();

            var doc = await NetworkUtils.LoadDocument(TIMETABLE_URL);
            var groupNodes = doc.GetElementbyId("vr").ChildNodes;

            foreach (var node in groupNodes.Where(node => node.Name == "option"))
            {
                groups.Add(new Group(node.GetAttributeValue(ATTR_VALUE, ""), node.InnerText));
            }

            return groups;
        }

        /// <summary>
        /// Use only for debug purposes
        /// </summary>
        public static async Task<ObservableCollection<Day>> GetTimetableByGroup(string group)
        {
            return await GetTimetableByGroup(group, new DateRange(DateUtils.ToDate("01.04.2015"), DateUtils.ToDate("12.04.2015")));
        }

        public static async Task<ObservableCollection<Day>> GetTimetableByGroup(string group, DateRange dateRange)
        {
            var parameters = new Dictionary<string, string>
            {
                { "rel", REL_GROUP },
                { "vr", group },
                { "from", DateUtils.ToString(dateRange.From) },
                { "to", DateUtils.ToString(dateRange.To) },
                { "submit_button", "ПОКАЗАТЬ" }
            };

            var doc = await NetworkUtils.LoadDocument(TIMETABLE_URL, "POST", parameters);
            var tableNodes = doc.GetElementbyId("send").ChildNodes;

            var nodes = new List<HtmlNode>();

            foreach (var node in tableNodes)
            {
                if (node.Name == "tr")
                {
                    foreach (var childNode in node.ChildNodes)
                    {
                        if (childNode.Attributes["class"] != null && childNode.Attributes["class"].Value.Contains("hours"))
                        {
                            nodes.Add(childNode);
                        }
                    }
                }
            }

            return getDays(nodes);
        }

        private static ObservableCollection<Day> getDays(List<HtmlNode> nodes)
        {
            var days = new ObservableCollection<Day>();

            if (nodes.Count == 1)
            {
                return days;
            }

            var i = 0;
            while (i < nodes.Count)
            {
                var date = nodes[i].InnerText;
                if (DateUtils.IsDate(date))
                {
                    var day = new Day(date);
                    i++;
                    do
                    {
                        var parameters = new List<string>();

                        for (var j = 0; j < COL_COUNT; j++)
                        {
                            parameters.Add(nodes[i].InnerText);
                            i++;
                        }

                        day.Lessons.Add(new Lesson(parameters));
                    }
                    while (i < nodes.Count && !DateUtils.IsDate(nodes[i].InnerText));

                    days.Add(day);
                }
            }

            return days;
        }
    }
}
