using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DJL.Work.BackWeb.Models.Permission
{
    public class ShowUserModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string IsDeleted { get; set; }

        public string Remark { get; set; }
        public string CreateTime { get; set; }

    }
}