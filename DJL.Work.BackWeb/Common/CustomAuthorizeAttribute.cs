using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DJL.Work.BackWeb.Common
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private string[] CookieRoles
        {
            get
            {
                if (HttpContext.Current.User != null)
                {
                    var formIdentity = HttpContext.Current.User.Identity as FormsIdentity;
                    if (formIdentity != null)
                    {
                        if (formIdentity.Ticket != null)
                        {
                            if (!string.IsNullOrEmpty(formIdentity.Ticket.UserData))
                            {
                                return formIdentity.Ticket.UserData.Split('|');
                            }
                        }
                    }
                }
                throw new ArgumentNullException("FormsIdentityRole");
            }
        }

        public CustomAuthorizeAttribute()
        {
            var roleName = ConfigurationManager.AppSettings.Get("DefaultRoleName");
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException("DefaultRoleName");
            this.Roles = roleName;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            if (!CookieRoles.Any()) return false;
            if (!CookieRoles.Any(x => x.Equals(this.Roles, StringComparison.CurrentCultureIgnoreCase)))
            {
                return false;
            }
            return base.AuthorizeCore(httpContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var js = string.Format(@"<script>$.messager.alert('{0}','{1}');</script>", "警告", "对不起,您的角色权限验证失败，请联系超级管理员!o(︶︿︶)o 唉");
            filterContext.Result = new ContentResult() { Content = js, ContentEncoding = Encoding.UTF8 };
        }
    }
}