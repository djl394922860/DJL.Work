using DJL.Work.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DJL.Work.Web.Common
{
    public class CustomErrorHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var exp = filterContext.Exception;
            if (exp != null)
            {
                LogHelper.LogWriterMsg(exp.Message);
            }
            base.OnException(filterContext);
        }
    }
}