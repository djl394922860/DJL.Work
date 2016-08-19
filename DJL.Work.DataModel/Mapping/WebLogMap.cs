using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace DJL.Work.DataModel.Mapping
{
    public class WebLogMap : EntityTypeConfiguration<WebLog>
    {
        public WebLogMap()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Message).HasColumnName("Message").IsMaxLength().IsRequired().IsVariableLength();
            this.Property(x => x.CreateTime).HasColumnName("CreateTime").IsRequired().HasColumnType("datetime2").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.ToTable("WebLog");
        }
    }
}
