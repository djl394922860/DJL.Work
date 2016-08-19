using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.DataModel
{
    public class MemberCollector
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MemberId { get; set; }

        public virtual Member Member { get; set; }
    }
}
