using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DJL.Work.Web.Models
{
    public class RegisterModel
    {
        [Required(AllowEmptyStrings=false,ErrorMessage="邮箱必须填,你绕过前台想干嘛")]
        [RegularExpression(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$", ErrorMessage = "你绕过前台想干嘛")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings=false,ErrorMessage="你绕过前台想干嘛")]
        [StringLength(10,MinimumLength=2)]
        public string Name { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(18)]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(18)]
        public string Password2 { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(6)]
        public string Vcode { get; set; }
    }
}