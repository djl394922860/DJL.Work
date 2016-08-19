using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DJL.Work.BackWeb.Models
{
    public class UpdateAdminInfoModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string OldPwd { get; set; }

        public string NewPwd { get; set; }
        public string ConfirmPwd { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Remark { get; set; }

        [Required]
        public bool IsUpdatedPwd { get; set; }
    }
}