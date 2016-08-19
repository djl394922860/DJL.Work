using System.Web.Mvc;

namespace DJL.Work.Web.Areas.Person
{
    public class PersonAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Person";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Person_default",
                "Person/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "DJL.Work.Web.Areas.Person.Controllers" }
            );
        }
    }
}