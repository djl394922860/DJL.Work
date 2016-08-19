using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DJL.Work.DataModel.Mapping
{
    public class MusicTypeMap : EntityTypeConfiguration<MusicType>
    {
        public MusicTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TypeName)
                .IsRequired()
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("MusicType");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TypeName).HasColumnName("TypeName").IsRequired().HasMaxLength(32);
            this.Property(x => x.Description).HasColumnName("Description").IsRequired().IsMaxLength();
            this.Property(x => x.Remark).IsRequired().HasColumnName("Remark").IsMaxLength();
        }
    }
}
