using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JQGrid4U.BL;

namespace JQGrid4U.Controllers
{
	public class DboardController : Controller
	{
		DashBoardBusinessLogic DashBoardBL = new DashBoardBusinessLogic();
        UserBusinessLogic UserBL = new UserBusinessLogic();

		[SessionExpire]
		public ActionResult Index()
		{
			if (Session["user"] == null)
			{
				return Redirect("~/Account/login");
			}
			else
			{

				List<DashBoard> objList = DashBoardBL.DashBoards.ToList();

				return View(objList);
				//return View();
			}
		}
		//Get Dashboard List
		public JsonResult SelectDashBoard()
		{

            int sess_userid = Convert.ToInt32(Session["userid"].ToString());

			return Json(DashBoardBL.DashBoards.ToList(), JsonRequestBehavior.AllowGet);

		}

        //Get Dashboard Parameter
        public JsonResult SelectDashBoardParam()
        {

            int sess_userid = Convert.ToInt32(Session["userid"].ToString());

            return Json(DashBoardBL.DashBoardParam(sess_userid).ToList(), JsonRequestBehavior.AllowGet);


        }



    }
}