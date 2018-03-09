using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TestProjectUber.DbLayer.EntityConfigurations;
using TestProjectUber.Services;

namespace TestProjectUber.DbLayer
{
    public class CoursesContext : DbContext, ICoursesContext
    {
        public CoursesContext() : base("name=CoursesContext") { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<CalendarEntry> CalendarEntries { get; set; }
        public DbSet<Hour> Hours { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CourseConfiguration());
            modelBuilder.Configurations.Add(new CalendarEntryConfiguration());
            modelBuilder.Configurations.Add(new HourConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}