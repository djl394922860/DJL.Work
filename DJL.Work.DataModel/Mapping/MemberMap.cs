using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DJL.Work.DataModel.Mapping
{
    public class MemberMap : EntityTypeConfiguration<Member>
    {
        public MemberMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.NickName)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.LoginEmail)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.Password)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("Member");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.NickName).HasColumnName("NickName");
            this.Property(t => t.LoginEmail).HasColumnName("LoginEmail");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.ActiveStatus).HasColumnName("ActiveStatus");
        }
    }
}
