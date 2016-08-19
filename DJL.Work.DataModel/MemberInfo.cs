using System;
using System.Collections.Generic;

namespace DJL.Work.DataModel
{
    public partial class MemberInfo
    {
        public int Id { get; set; }
        public Nullable<byte> Age { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<bool> Sex { get; set; }
        public string HeadImageUrl { get; set; }
        public string Phone { get; set; }
        public string Hobby { get; set; }
        public virtual Member Member { get; set; }
    }
}
