using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProjectUber.DbLayer
{
    public class Course
    {
        public Course()
        {
            CalendarEntries = new HashSet<CalendarEntry>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public long Value { get; set; }
        
        public virtual ICollection<CalendarEntry> CalendarEntries { get; set; }
    }
}