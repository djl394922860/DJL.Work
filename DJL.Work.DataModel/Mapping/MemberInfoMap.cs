using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DJL.Work.DataModel.Mapping
{
    public class MemberInfoMap : EntityTypeConfiguration<MemberInfo>
    {
        public MemberInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HeadImageUrl)
                .HasMaxLength(128);

            this.Property(t => t.Phone)
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("MemberInfo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Age).HasColumnName("Age");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.HeadImageUrl).HasColumnName("HeadImageUrl");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Hobby).HasColumnName("Hobby");

            // Relationships
            this.HasRequired(t => t.Member)
                .WithOptional(t => t.MemberInfo);
            //WithRequiredDependent
            //WithRequiredPrincipal
        }
    }
}
