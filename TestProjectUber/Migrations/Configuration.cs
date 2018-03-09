namespace TestProjectUber.Migrations
{
    using DbLayer;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TestProjectUber.DbLayer.CoursesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TestProjectUber.DbLayer.CoursesContext context)
        {
            var hours = new Dictionary<string, Hour>
            {
                {"09:00", new Hour {Id = 1, Value = "09:00"}},
                {"10:00", new Hour {Id = 2, Value = "10:00"}},
                {"11:00", new Hour {Id = 3, Value = "11:00"}},
                {"12:00", new Hour {Id = 4, Value = "12:00"}},
                {"13:00", new Hour {Id = 5, Value = "13:00"}},
                {"14:00", new Hour {Id = 6, Value = "14:00"}},
                {"15:00", new Hour {Id = 7, Value = "15:00"}},
                {"16:00", new Hour {Id = 8, Value = "16:00"}},
                {"17:00", new Hour {Id = 9, Value = "17:00"}}
            };

            foreach (var hour in hours.Values)
                context.Hours.AddOrUpdate(h => h.Id, hour);
            
            var courses = new Dictionary<string, Course>
            {
                {"ASP.NET", new Course {Id = 1, Name = "ASP.NET", Value = 10000}},
                {"JavaScript", new Course {Id = 2, Name = "JavaScript", Value = 8000}},
                {"PHP", new Course {Id = 3, Name = "PHP", Value = 8500}},
                {"Python", new Course {Id = 4, Name = "Python", Value = 11000}}
            };

            foreach (var course in courses.Values)
                context.Courses.AddOrUpdate(c => c.Id, course);

            var calendarEntries = new List<CalendarEntry>
            {
                new CalendarEntry
                {
                    Id = 1,
                    Day = Enumerations.Days.Monday,
                    HourId = hours["15:00"].Id,
                    CourseId = courses["ASP.NET"].Id
                },
                new CalendarEntry
                {
                    Id = 2,
                    Day = Enumerations.Days.Tuesday,
                    HourId = hours["10:00"].Id,
                    CourseId = courses["JavaScript"].Id
                },
                new CalendarEntry
                {
                    Id = 3,
                    Day = Enumerations.Days.Wednesday,
                    HourId = hours["13:00"].Id,
                    CourseId = courses["PHP"].Id
                },
                new CalendarEntry
                {
                    Id = 4,
                    Day = Enumerations.Days.Thursday,
                    HourId = hours["17:00"].Id,
                    CourseId = courses["Python"].Id
                },
                new CalendarEntry
                {
                    Id = 5,
                    Day = Enumerations.Days.Friday,
                    HourId = hours["16:00"].Id,
                    CourseId = courses["JavaScript"].Id
                }
            };
            foreach (var calendarEntry in calendarEntries)
            {
                context.CalendarEntries.AddOrUpdate(ce => ce.Id, calendarEntry);
            }
        }
    }
}
