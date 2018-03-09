using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace TestProjectUber.DbLayer.EntityConfigurations
{
    public class HourConfiguration : EntityTypeConfiguration<Hour>
    {
        public HourConfiguration()
        {
            Property(c => c.Value)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(5);
        }
    }
}