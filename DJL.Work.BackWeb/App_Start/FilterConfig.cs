using DJL.Work.BackWeb.Common;
using System.Web;
using System.Web.Mvc;

namespace DJL.Work.BackWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomErrorHandlerAttribute());
        }
    }
}
