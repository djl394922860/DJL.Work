using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DJL.Work.DataModel.Mapping
{
    public class MusicPlayerMap : EntityTypeConfiguration<MusicPlayer>
    {
        public MusicPlayerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.CountryName)
                .HasMaxLength(32).IsRequired();

            // Table & Column Mappings
            this.ToTable("MusicPlayer");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name").HasMaxLength(32);
            this.Property(t => t.Birthday).HasColumnName("Birthday").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnType("datetime2").IsRequired();
            this.Property(t => t.Sex).HasColumnName("Sex").IsOptional();
            this.Property(t => t.Description).HasColumnName("Description").IsRequired().IsMaxLength();
            this.Property(t => t.CountryName).HasColumnName("CountryName");
            this.Property(t => t.Hobby).HasColumnName("Hobby").IsRequired();
        }
    }
}
