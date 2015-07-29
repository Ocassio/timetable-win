using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Timetable.Utils;

namespace Timetable.Models
{
    [DataContract]
    public class Day
    {
        [DataMember]
        private List<Lesson> lessons;

        [DataMember]
        private string date;

        public string Date
        {
            get { return date; }

            set
            {
                date = value;
                DayOfWeek = DateUtils.GetDayOfTheWeek(date);
            }
        }

        public string DayOfWeek { get; set; }

        public List<Lesson> Lessons
        {
            get
            {
                if (lessons == null)
                {
                    lessons = new List<Lesson>();
                }

                return lessons;
            }

            set
            {
                lessons = value;
            }
        }

        public Day() { }

        public Day(string date)
        {
            Date = date;
            DayOfWeek = DateUtils.GetDayOfTheWeek(date);
        }

        public Day(string date, List<Lesson> lessons)
        {
            Date = date;
            DayOfWeek = DateUtils.GetDayOfTheWeek(date);
            Lessons = lessons;
        }

        public override string ToString()
        {
//            string result = Date + "\n";
//
//            foreach (Lesson lesson in Lessons)
//            {
//                result += lesson.ToString() + "\n";
//            }
//
//            return result;

            return Date;
        }
    }
}
