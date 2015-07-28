using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.Models
{
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
    }
}
