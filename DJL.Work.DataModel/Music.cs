using System;
using System.Collections.Generic;

namespace DJL.Work.DataModel
{
    public partial class Music
    {
        public Music()
        {
            MusicPlayers = new List<MusicPlayer>();
            MusicTypes = new List<MusicType>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string Description { get; set; }
        public string BackgroundImgRelativeUrl { get; set; }
        public string BackgroundImgPhysicalUrl { get; set; }
        public System.DateTime UploadTime { get; set; }
        public System.DateTime PublishTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public Nullable<byte> Language { get; set; }
        public string SourceRelativeUrl { get; set; }
        public string SourcePhysicalUrl { get; set; }
        public string LrcRelativeUrl { get; set; }
        public string LrcPhysicalUrl { get; set; }
        public int DownloadCount { get; set; }
        public int AccessTimes { get; set; }
        public int PlayCount { get; set; }
        public virtual ICollection<MusicPlayer> MusicPlayers { get; set; }
        public virtual ICollection<MusicType> MusicTypes { get; set; }
    }
}
