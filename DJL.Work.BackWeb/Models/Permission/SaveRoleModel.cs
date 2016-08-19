using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DJL.Work.BackWeb.Models.Permission
{
    public class SaveRoleModel
    {
        [Required]
        public string RoleName { get; set; }

        [Required]
        public string Available { get; set; }

        public int[] RoleActions { get; set; }

        [Required]
        public string Remark { get; set; }
    }
}