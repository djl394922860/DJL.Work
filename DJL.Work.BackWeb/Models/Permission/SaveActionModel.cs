using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DJL.Work.BackWeb.Models.Permission
{
    public class SaveActionModel
    {
        [Required]
        public string ActionName { get; set; }

        [Required]
        public string Available { get; set; }

        public byte ActionMethod { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 6)]
        public string ActionRequestUrl { get; set; }

        [Required]
        public string Remark { get; set; }
    }
}