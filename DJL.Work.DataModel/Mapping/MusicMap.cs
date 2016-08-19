using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DJL.Work.DataModel.Mapping
{
    public class MusicMap : EntityTypeConfiguration<Music>
    {
        public MusicMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(32).HasColumnName("Name");

            this.Property(t => t.BackgroundImgPhysicalUrl)
                .IsRequired().IsMaxLength().HasColumnName("BackgroundImgPhysicalUrl");

            this.Property(x => x.AccessTimes).IsRequired().HasColumnName("AccessTimes");

            this.Property(x => x.BackgroundImgRelativeUrl).IsRequired().HasColumnName("BackgroundImgRelativeUrl").IsMaxLength();

            this.Property(t => t.SourcePhysicalUrl)
                .IsRequired().IsMaxLength().HasColumnName("SourcePhysicalUrl");

            this.Property(x => x.SourceRelativeUrl).IsRequired().IsMaxLength().HasColumnName("SourceRelativeUrl");

            this.Property(t => t.LrcPhysicalUrl).IsRequired().IsMaxLength().HasColumnName("LrcPhysicalUrl");

            this.Property(x => x.LrcRelativeUrl).IsRequired().IsMaxLength().HasColumnName("LrcRelativeUrl");

            // Table & Column Mappings
            this.ToTable("Music");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Remark).HasColumnName("Remark").IsRequired().IsMaxLength();
            this.Property(x => x.Description).HasColumnName("Description").IsRequired().IsMaxLength();
            this.Property(t => t.UploadTime).HasColumnName("UploadTime").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnType("datetime2");
            this.Property(t => t.PublishTime).HasColumnName("PublishTime").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnType("datetime2");
            this.Property(x => x.UpdateTime).HasColumnType("datetime2").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnName("UpdateTime");
            this.Property(t => t.Language).HasColumnName("Language").IsOptional();
            this.Property(t => t.DownloadCount).HasColumnName("DownloadCount");
            this.Property(t => t.AccessTimes).HasColumnName("AccessTimes");
            this.Property(x => x.PlayCount).HasColumnName("PlayCount");

            // Relationships
            this.HasMany<MusicPlayer>(m => m.MusicPlayers)
                .WithMany(p => p.Musics)
                .Map(mp =>
                {
                    mp.MapLeftKey("MusicRefId");
                    mp.MapRightKey("PlayerRefId");
                    mp.ToTable("MusicWithPlayer");
                });

            this.HasMany<MusicType>(m => m.MusicTypes)
                .WithMany(t => t.Musics)
                .Map(mt =>
                {
                    mt.MapLeftKey("MusicRefId");
                    mt.MapRightKey("TypeRefId");
                    mt.ToTable("MusicWithType");
                });
        }
    }
}
