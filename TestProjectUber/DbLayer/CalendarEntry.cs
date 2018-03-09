using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestProjectUber.Enumerations;

namespace TestProjectUber.DbLayer
{
    public class CalendarEntry
    {
        public int Id { get; set; }
        public Days Day { get; set; }
        public int HourId { get; set; }
        public int CourseId { get; set; }

        public virtual Hour Hour { get; set; }
        public virtual Course Course { get; set; }
    }
}