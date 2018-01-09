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
    public class ConveyGraphFilterController : Controller
    {

        ConveyorBusinessLogic ConveyorBL = new ConveyorBusinessLogic();

        // GET: ConveyGraphFilter
        public ActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public JsonResult FilterConveyor(int conveyorid = 0, string device = null)
        {
            device = device.ToLower();

            return Json(ConveyorBL.Conveyors.Where(p => p.conveyorID == conveyorid && p.deviceID.ToLower().Contains(device)).ToList(), JsonRequestBehavior.AllowGet);
           

        }


    }
}