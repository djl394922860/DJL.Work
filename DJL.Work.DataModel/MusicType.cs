using System;
using System.Collections.Generic;

namespace DJL.Work.DataModel
{
    public partial class MusicType
    {
        public MusicType()
        {
            this.Musics = new List<Music>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Remark { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Music> Musics { get; set; }
    }
}
