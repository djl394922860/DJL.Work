using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace DJL.Work.DataModel.Mapping
{
    public class AdminInfoMap : EntityTypeConfiguration<AdminInfo>
    {
        public AdminInfoMap()
        {
            // Primary Key
            this.HasKey(x => x.ID);

            // Properties
            this.Property(x => x.ID).HasColumnName("ID");
            this.Property(x => x.UserName).IsRequired().HasMaxLength(32).HasColumnName("UserName");
            this.Property(x => x.Password).IsRequired().HasMaxLength(32).HasColumnName("Password");
            this.Property(x => x.Email).IsRequired().HasMaxLength(32).HasColumnName("Email");
            this.ToTable("AdminInfo");
            this.Property(x => x.CreateTime).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnName("CreateTime").HasColumnType("datetime2");
            this.Property(x => x.DeleteFlag).IsRequired().HasColumnName("DeleteFlag");
            this.Property(x => x.Remark).IsRequired().HasColumnName("Remark").HasMaxLength(512);
        }
    }
}
