using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Models
{
    [DataContract]
    public class Group
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
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

        public override bool Equals(object obj)
        {
            if (typeof (Group) != obj.GetType()) return false;
            var other = (Group) obj;
            return Id == other.Id;
        }
    }
}
