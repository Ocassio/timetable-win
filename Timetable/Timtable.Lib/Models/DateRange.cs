using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Timetable.Utils;

namespace Timetable.Models
{
    [DataContract]
    public class DateRange
    {
        private DateTime from;
        private DateTime to;

        public DateRange()
        {
        }

        public DateRange(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        [DataMember]
        public DateTime From
        {
            get { return from; }
            set
            {
                if (value != null && to != null && value > to)
                {
                    to = value;
                }
                from = value;
            }
        }

        [DataMember]
        public DateTime To
        {
            get { return to; }
            set
            {
                if (value != null && from != null && from > value)
                {
                    from = value;
                }
                to = value;
            }
        }

        public override string ToString()
        {
            return DateUtils.ToString(From) + " - " + DateUtils.ToString(To);
        }
    }
}
