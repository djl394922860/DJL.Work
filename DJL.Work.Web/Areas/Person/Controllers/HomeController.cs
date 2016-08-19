using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DJL.Work.Web.Areas.Person.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //
        // GET: /Person/Home/

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NotFind()
        {
            return View();
        }
	}
}