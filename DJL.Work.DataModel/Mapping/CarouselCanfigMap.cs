using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJL.Work.DataModel.Mapping
{
    public class CarouselCanfigMap : EntityTypeConfiguration<CarouselCanfig>
    {
        public CarouselCanfigMap()
        {
            // Primary Key            
            this.HasKey(x => x.Id);

            // Properties
            this.ToTable("CarouselCanfig");
            this.Property(x => x.ImageDataSize).HasColumnName("ImageDataSize").IsRequired();
            this.Property(x => x.MusicDescription).HasColumnName("MusicDescription").IsRequired().HasMaxLength(32);
            this.Property(x => x.MusicName).IsRequired().HasMaxLength(32).HasColumnName("MusicName");
            this.Property(x => x.Order).HasColumnName("Order").IsRequired();
            this.Property(x => x.ImageFormat).HasColumnName("ImageFormat").IsRequired().HasMaxLength(16);
            this.Property(x => x.UpLoadTime).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnName("UpLoadTime").HasColumnType("datetime2");
            this.Property(x => x.IsDeleted).IsRequired().HasColumnName("IsDeleted").HasColumnType("bit");
            this.Property(x => x.UpdateTime).IsRequired().HasColumnName("UpdateTime").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnType("datetime2");
            this.Property(x => x.NetworkRelativePath).IsRequired().HasColumnName("NetworkRelativePath").IsMaxLength();
            this.Property(x => x.PhysicalPath).IsRequired().HasColumnName("PhysicalPath").IsMaxLength();
        }
    }
}
