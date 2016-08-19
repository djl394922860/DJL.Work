using DJL.Work.BackWeb.App_Start;
using DJL.Work.DataModel;
using DJL.Work.Enum;
using DJL.Work.ICommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DJL.Work.BackWeb.Common.PermissionAttribute
{
    /// <summary>
    /// 主要用于写操作权限控制(Post)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class PermissionPostJsonAttribute : ActionFilterAttribute
    {
        private string UserId
        {
            get
            {
                if (HttpContext.Current.User != null)
                {
                    var formIdentity = HttpContext.Current.User.Identity as FormsIdentity;
                    if (formIdentity != null)
                    {
                        return formIdentity.Name;
                    }
                }
                throw new ArgumentNullException("FormsIdentityUser");
            }
        }

        private string[] Roles
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

        private static readonly IRoleInfoService _roleInfoService = UnityConfig.Instance.GetService<IRoleInfoService>();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //check permission
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            var typeResult = filterContext.Controller.GetType().GetMethod(filterContext.ActionDescriptor.ActionName).ReturnType;
            if (typeResult != typeof(JsonResult))
            {
                throw new Exception("代码标签使用错误，请通知开发人员");
            }
            var std = new StandardResult();
            if (string.IsNullOrEmpty(UserId))
            {
                std.message = "找不到登录用户，请检查是否已退出系统";
                filterContext.Result = new JsonResult() { Data = std, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
            }
            if (!Roles.Any())
            {
                std.message = "找不到登录用户任何角色，请检查是否拥有权限访问";
                filterContext.Result = new JsonResult() { Data = std, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
            }
            //get all role->action
            var permissions = _roleInfoService.GetActionsByRoleNames(Roles);
            var actionName = filterContext.ActionDescriptor.ActionName;
            var controllterName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var url = string.Format(@"/{0}/{1}", controllterName, actionName);
            var method = filterContext.RequestContext.HttpContext.Request.HttpMethod;
            var type = typeof(BaseEnum);
            //compare
            if (!permissions.Any(x => string.Equals(type.GetEnumName(x.HttpMethod), method, StringComparison.CurrentCultureIgnoreCase) && string.Equals(x.RequestUrl, url, StringComparison.CurrentCultureIgnoreCase)))
            {
                std.message = "对不清，您的权限不足，操作失败";
                filterContext.Result = new JsonResult() { Data = std, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
            }
            base.OnActionExecuting(filterContext);
        }
    }
}