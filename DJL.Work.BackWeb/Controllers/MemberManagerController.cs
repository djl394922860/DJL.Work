using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DJL.Work.BackWeb.Controllers
{
    [Authorize]
    public class MemberManagerController : Controller
    {
        //
        // GET: /MemberManager/

        #region Search
        [HttpGet]
        public ActionResult Search()
        {
            return PartialView();
        }

        #endregion

        #region SealManager
        [HttpGet]
        public ActionResult SealManager()
        {
            return PartialView();
        }
        #endregion

    }
}