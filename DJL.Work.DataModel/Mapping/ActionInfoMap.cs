using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace DJL.Work.DataModel.Mapping
{
    public class ActionInfoMap : EntityTypeConfiguration<ActionInfo>
    {
        public ActionInfoMap()
        {
            // Primary Key
            this.HasKey(x => x.ID);

            // Properties
            this.Property(x => x.ID).HasColumnName("ID");
            this.Property(x => x.ActionName).IsRequired().HasMaxLength(32).HasColumnName("ActionName");
            this.Property(x => x.RequestUrl).IsRequired().HasMaxLength(512).HasColumnName("RequestUrl");
            this.Property(x => x.HttpMethod).IsRequired().HasColumnName("HttpMethod");
            this.Property(x => x.Remark).IsRequired().HasMaxLength(512).HasColumnName("Remark");
            this.Property(x => x.DeleteFlag).IsRequired().HasColumnName("DeleteFlag");
            this.Property(x => x.CreateTime).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnName("CreateTime").HasColumnType("datetime2");
            this.ToTable("ActionInfo");
        }
    }
}
