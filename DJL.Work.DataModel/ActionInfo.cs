using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.DataModel
{
    public class ActionInfo
    {
        public ActionInfo()
        {
            CreateTime = DateTime.Now;
            RoleInfos = new List<RoleInfo>();
            DeleteFlag = false;
        }
        public int ID { get; set; }
        public string ActionName { get; set; }
        public string RequestUrl { get; set; }
        public byte HttpMethod { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public bool DeleteFlag { get; set; }

        public virtual ICollection<RoleInfo> RoleInfos { get; set; }
    }
}
