using System;
using System.Collections.Generic;

namespace DJL.Work.DataModel
{
    public partial class MusicPlayer
    {
        public MusicPlayer()
        {
            this.Musics = new List<Music>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime Birthday { get; set; }
        public Nullable<bool> Sex { get; set; }
        public string Description { get; set; }
        public string CountryName { get; set; }
        public string Hobby { get; set; }
        public virtual ICollection<Music> Musics { get; set; }
    }
}
