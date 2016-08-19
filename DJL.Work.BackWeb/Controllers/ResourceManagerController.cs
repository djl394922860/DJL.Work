using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DJL.Work.BackWeb.Controllers
{
    [Authorize]
    public class ResourceManagerController : Controller
    {
        //
        // GET: /ResourceManager/

        #region MusicManager
        [HttpGet]
        public ActionResult MusicManager()
        {
            return PartialView();
        }

        #endregion

        #region MusicTypeManager
        [HttpGet]
        public ActionResult MusicTypeManager()
        {
            return PartialView();
        }

        #endregion

        #region MusicLycManager
        [HttpGet]
        public ActionResult MusicLycManager()
        {
            return PartialView();
        }
        #endregion

        #region MusicPlayerManager
        [HttpGet]
        public ActionResult MusicPlayerManager()
        {
            return PartialView();
        }
        #endregion

    }
}