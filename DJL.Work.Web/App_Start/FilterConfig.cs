using DJL.Work.Web.Common;
using System.Web;
using System.Web.Mvc;

namespace DJL.Work.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomErrorHandlerAttribute());
        }
    }
}
