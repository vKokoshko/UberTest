using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace TestProjectUber.DbLayer.EntityConfigurations
{

    public class CourseConfiguration : EntityTypeConfiguration<Course>
    {
        public CourseConfiguration()
        {
            Property(c => c.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            Property(c => c.Value)
                .IsRequired();
        }
    }
}