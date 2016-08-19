using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace DJL.Work.DataModel.Mapping
{
    public class MemberCollectorMap : EntityTypeConfiguration<MemberCollector>
    {
        public MemberCollectorMap()
        {
            // Primary Key
            this.HasKey(x => x.Id);

            // Properties
            this.Property(x => x.Name).IsRequired().HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("MemberCollector");
            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.Name).HasColumnName("Name");
            this.Property(x => x.MemberId).HasColumnName("MemberId");

            // Relationships
            this.HasRequired(x => x.Member)
                .WithMany(x => x.MemberCollectors)
                .HasForeignKey(x => x.MemberId);//.WillCascadeOnDelete(bool value) 是否启动级联删除
        }
    }
}
