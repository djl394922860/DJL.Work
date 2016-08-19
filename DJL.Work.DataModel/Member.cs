using System;
using System.Collections.Generic;

namespace DJL.Work.DataModel
{
    public partial class Member
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string LoginEmail { get; set; }
        public string Password { get; set; }
        public bool ActiveStatus { get; set; }
        public virtual MemberInfo MemberInfo { get; set; }
        public virtual ICollection<MemberCollector> MemberCollectors { get; set; }
    }
}
