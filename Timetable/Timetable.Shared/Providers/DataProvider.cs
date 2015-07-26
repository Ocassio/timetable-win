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
            ObservableCollection<Group> groups = new ObservableCollection<Group>();

            HtmlDocument doc = await NetworkUtils.LoadDocument(TIMETABLE_URL);
            HtmlNodeCollection groupNodes = doc.GetElementbyId("vr").ChildNodes;

            foreach (HtmlNode node in groupNodes.Where(node => node.Name == "option"))
            {
                groups.Add(new Group(node.GetAttributeValue(ATTR_VALUE, ""), node.InnerText));
            }

            return groups;
        }

        public static async Task<ObservableCollection<Day>> GetTimetableByGroup(string group)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("rel", REL_GROUP);
            parameters.Add("vr", group);
            parameters.Add("from", "01.04.15"); //TODO change date
            parameters.Add("to", "12.04.15"); //TODO change date
            parameters.Add("submit_button", "ПОКАЗАТЬ");

            HtmlDocument doc = await NetworkUtils.LoadDocument(TIMETABLE_URL, "POST", parameters);
            HtmlNodeCollection tableNodes = doc.GetElementbyId("send").ChildNodes;

            List<HtmlNode> nodes = new List<HtmlNode>();

            foreach (HtmlNode node in tableNodes)
            {
                if (node.Name == "tr")
                {
                    foreach (HtmlNode childNode in node.ChildNodes)
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
            ObservableCollection<Day> days = new ObservableCollection<Day>();

            if (nodes.Count == 1)
            {
                return days;
            }

            int i = 0;
            while (i < nodes.Count)
            {
                string date = nodes[i].InnerText;
                if (DateUtils.IsDate(date))
                {
                    Day day = new Day(date);
                    i++;
                    do
                    {
                        List<string> parameters = new List<string>();

                        for (int j = 0; j < COL_COUNT; j++)
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
