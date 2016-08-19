using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DJL.Work.BackWeb.Models.Permission
{
    public class SaveUserModel
    {
        [Required]
        [StringLength(32, MinimumLength = 1)]
        public string UserName { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 1)]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string UserNewPwd { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string UserConfirmPwd { get; set; }

        [Required]
        public string Available { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Remark { get; set; }

        //roles
        public int[] UserRoles { get; set; }
    }
}