using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Models
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
            //byte[] byteName = Encoding.GetEncoding(1251).GetBytes(Name);
            //byteName = Encoding.Convert(Encoding.GetEncoding(1251), Encoding.UTF8, byteName);
            return Id + " - " + Name;//Encoding.UTF8.GetString(byteName);
        }
    }
}
