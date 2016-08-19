using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.DataModel
{
    public class RoleInfo
    {
        public RoleInfo()
        {
            AdminInfos = new List<AdminInfo>();
            ActionInfos = new List<ActionInfo>();
            DeleteFlag = false;
            CreateTime = DateTime.Now;
        }
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public bool DeleteFlag { get; set; }

        public virtual ICollection<AdminInfo> AdminInfos { get; set; }
        public virtual ICollection<ActionInfo> ActionInfos { get; set; }
    }
}
