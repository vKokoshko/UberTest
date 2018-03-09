using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProjectUber.DbLayer
{
    public class Hour
    {
        public Hour()
        {
            CalendarEntries = new HashSet<CalendarEntry>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public virtual ICollection<CalendarEntry> CalendarEntries { get; set; }
    }
}