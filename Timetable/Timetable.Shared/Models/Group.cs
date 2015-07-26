﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Models
{
    public class Group
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Group() { }

        public Group(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
