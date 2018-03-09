using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProjectUber.DbLayer;

namespace TestProjectUber.Services
{
    public interface ICoursesContext
    {
        DbSet<Course> Courses { get; set; }
        DbSet<CalendarEntry> CalendarEntries { get; set; }
        DbSet<Hour> Hours { get; set; }

        int SaveChanges();
    }
}
