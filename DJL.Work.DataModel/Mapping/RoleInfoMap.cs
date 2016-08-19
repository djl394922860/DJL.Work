using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace DJL.Work.DataModel.Mapping
{
    public class RoleInfoMap : EntityTypeConfiguration<RoleInfo>
    {
        public RoleInfoMap()
        {
            // Primery Key
            this.HasKey(x => x.ID);

            // Properties
            this.Property(x => x.ID).HasColumnName("ID");
            this.Property(x => x.RoleName).IsRequired().HasMaxLength(32).HasColumnName("RoleName");
            this.Property(x => x.Remark).IsRequired().HasMaxLength(512).HasColumnName("Remark");
            this.Property(x => x.CreateTime).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnName("CreateTime").HasColumnType("datetime2");
            this.Property(x => x.DeleteFlag).IsRequired().HasColumnName("DeleteFlag");
            this.ToTable("RoleInfo");

            // Relationships
            this.HasMany<AdminInfo>(r => r.AdminInfos)
                .WithMany(a => a.RoleInfos)
                .Map(ra =>
                {
                    ra.MapLeftKey("RoleRefID");
                    ra.MapRightKey("AdminRefID");
                    ra.ToTable("RoleWithAdmin");
                });
            this.HasMany<ActionInfo>(r => r.ActionInfos)
                .WithMany(a => a.RoleInfos)
                .Map(ra =>
                {
                    ra.MapLeftKey("RoleRefID");
                    ra.MapRightKey("ActionRefID");
                    ra.ToTable("RoleWithAction");
                });
        }
    }
}
