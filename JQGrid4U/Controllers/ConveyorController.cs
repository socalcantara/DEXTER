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
    public class ConveyorController : Controller
    {
        // GET: Conveyor

        ConveyorBusinessLogic ConveyorBL = new ConveyorBusinessLogic();

        public ActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public JsonResult SelectConveyor(int conveyorId = 0)
        {


            return Json(ConveyorBL.Conveyors.Where(p => p.conveyorID == conveyorId).ToList(), JsonRequestBehavior.AllowGet);
            //return Json(MonitBL.Monits.Where(p => p.siteId == siteId).OrderBy(x => x.Status).ThenBy(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);

        }

        [AllowAnonymous]
        public JsonResult SelectConveyorAll()
        {


            return Json(ConveyorBL.Conveyors.ToList(), JsonRequestBehavior.AllowGet);
            //return Json(MonitBL.Monits.Where(p => p.siteId == siteId).OrderBy(x => x.Status).ThenBy(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);

        }




    }
}