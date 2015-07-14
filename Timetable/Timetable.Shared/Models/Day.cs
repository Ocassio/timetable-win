using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Models
{
    public class Day
    {
        private List<Lesson> lessons;

        public string Date { get; set; }

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
        }

        public Day(string date, List<Lesson> lessons)
        {
            Date = date;
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
