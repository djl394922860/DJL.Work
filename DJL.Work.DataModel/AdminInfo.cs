using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.DataModel
{
    public class AdminInfo
    {
        public AdminInfo()
        {
            RoleInfos = new List<RoleInfo>();
            DeleteFlag = false;
            CreateTime = DateTime.Now;
        }
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime CreateTime { get; set; }
        public bool DeleteFlag { get; set; }
        public string Remark { get; set; }

        public virtual ICollection<RoleInfo> RoleInfos { get; set; }
    }
}
