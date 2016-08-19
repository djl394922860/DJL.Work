using DJL.Work.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DJL.Work.BackWeb.Common
{
    public class CustomErrorHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var exp = filterContext.Exception;
            if (exp != null)
            {
                var innerStr = string.Empty;
                if (exp.InnerException != null && !string.IsNullOrEmpty(exp.InnerException.Message))
                {
                    innerStr = exp.InnerException.Message;
                }
                var str = string.Format(@"异常信息:{0};\r\n内部异常信息:{1};\r\n调用堆栈信息:{2}\r\n", exp.Message, innerStr, exp.StackTrace);
                LogHelper.LogWriterMsg(str);
            }
            base.OnException(filterContext);
        }
    }
}