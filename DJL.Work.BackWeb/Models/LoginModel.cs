using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DJL.Work.BackWeb.Models
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "帐号是必须的!")]
        [StringLength(32, MinimumLength = 1, ErrorMessage = "帐号长度在1-32之间!")]
        public string AdminName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "密码是必须的!")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "密码长度在6-18之间!")]
        public string AdminPwd { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "验证码是必须的!")]
        [RegularExpression("^[0-9]{4,6}$", ErrorMessage = "验证码格式错误!")]
        public string ValidataCode { get; set; }
    }
}