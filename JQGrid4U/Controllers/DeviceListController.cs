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
    public class DeviceListController : Controller
    {

        MonitBusinessLogic MonitBL = new MonitBusinessLogic();

        // GET: DeviceList
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SelectDevice(int conveyorId = 0)
        {

            return Json(MonitBL.DeviceList(conveyorId).OrderBy(x => x.deviceID).ToList(), JsonRequestBehavior.AllowGet);
        }


    }
}