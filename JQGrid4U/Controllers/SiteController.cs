using JQGrid4U.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using JQGrid4U.BL;
using System.Data;

namespace JQGrid4U.Controllers
{
    public class SiteController : Controller

    {

        SiteBusinessLogic SiteBL = new SiteBusinessLogic();

        // GET: Site
        public ActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public JsonResult SelectSite()
        {


            return Json(SiteBL.Sites.ToList(), JsonRequestBehavior.AllowGet);


        }




    }
}