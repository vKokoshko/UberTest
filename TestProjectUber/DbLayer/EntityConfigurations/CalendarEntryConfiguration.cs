using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace TestProjectUber.DbLayer.EntityConfigurations
{
    public class CalendarEntryConfiguration : EntityTypeConfiguration<CalendarEntry>
    {
        public CalendarEntryConfiguration()
        {

            HasRequired(ce => ce.Hour)
                .WithMany(h => h.CalendarEntries)
                .HasForeignKey(ce => ce.HourId);

            HasRequired(ce => ce.Course)
                .WithMany(h => h.CalendarEntries)
                .HasForeignKey(ce => ce.CourseId);
        }
    }
}