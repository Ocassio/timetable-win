using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Models
{
    public class Lesson
    {
        public String Number { get; set; }
        public String Room { get; set; }
        public String Name { get; set; }
        public String Teacher { get; set; }
        public String Type { get; set; }

        public Lesson() { }

        public Lesson(string number, string room, string name, string teacher, string type)
        {
            Number = number;
            Room = room;
            Name = name;
            Teacher = teacher;
            Type = type;
        }

        public Lesson(List<string> parameters)
        {
            Room = parameters[0];
            Number = parameters[1];
            Teacher = parameters[2];
            Type = parameters[3];
            Name = parameters[4];
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3}, {4}", Number, Name, Room, Teacher, Type);
        }
    }
}
