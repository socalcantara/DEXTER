﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JQGrid4U.BL;
using System.Data;
using System.Text;

namespace JQGrid4U.Controllers
{
    [SessionExpire]
    public class RoleManagementController : Controller
    {
        // GET: RoleManagement Tables
        RoleManagement RoleManagementTables = new RoleManagement();

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetRolesData()
        {
            return Json(RoleManagementTables.RolesList.ToList(), JsonRequestBehavior.AllowGet);
        }
        //Add New Role 
        public JsonResult InsertRole(Roles role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (RoleManagementTables.InsertNewRole(role) > 0)
                        return Json(new { isError = "F", message = "New role has been added!" });
                    else
                        return Json(new { isError = "T", message = "Role already exist." });
                }
                catch (Exception ex)
                {
                    return Json(new { isError = "T", message = "Could not insert data." });
                }
            }
            else
            {
                return Json(new { isError = "T", message = "Could not insert data." });
            }
        }
        //Get List of Roles
        public string ListRoles()
        {
            StringBuilder sb = new StringBuilder();
            List<Roles> roles = RoleManagementTables.RolesList.ToList();
            foreach (var list in roles)
            {
                sb.Append("<option value=" + list.ID + ">" + list.RoleName + "</option>");
            }
            return sb.ToString();
        }
        //Menu Display to List
        public string AMMenus(int? roleid)
        {
            StringBuilder sb = new StringBuilder();
            List<RoleMenuAccess> role = RoleManagementTables.RoleMenuAcessList.Where(p => p.RoleID == roleid).ToList();
            List<MenuHdr> menuOne = RoleManagementTables.MenuList.Where(p => p.ParentID == null).ToList();//db.SubMenus.Where(p => p.parentMenuId == parentid).ToList();

            //sb.Append("<ul>");
            foreach (MenuHdr list in menuOne)
            {
                sb.Append("<li><a " + (role.Any(p => p.MenuHdrID == list.menuID) ? "class='jstree-clicked'" : "") + " value=" + list.menuID + " moduleType=\"MainMenu\">" + list.MenuText + "</a>");
                sb.Append(AMSecondMenu(list.menuID, roleid));
                sb.Append("</li>");

            }
            //sb.Append("</ul>");
            return sb.ToString();
        }
        //Get Child Menu
        public string AMSecondMenu(int? SecondMenu, int? roleid)
        {
            StringBuilder sb = new StringBuilder();
            List<MenuHdr> submenu = RoleManagementTables.MenuList.Where(p => p.ParentID == SecondMenu).ToList();
            //apprvrRole role = db.apprvrRoles.Single(p => p.roleID == roleid);
            List<RoleMenuAccess> role = RoleManagementTables.RoleMenuAcessList.Where(p => p.RoleID == roleid).ToList();
            sb.Append("<ul>");
            foreach (var list in submenu)
            {
                sb.Append("<li><a " + (role.Any(p => p.MenuHdrID == list.menuID) ? "class='jstree-clicked'" : "") + " value=" + list.menuID + " parentvalue=" + list.ParentID + " moduleType=\"2ndMenu\">" + list.MenuText + "</a>");
                sb.Append(AMSecondMenu(list.menuID, roleid));
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
        //Add Role to Menu
        public ActionResult AddRoleToMenu(RoleAccess roleAccess)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Delete Menu Access
                    RoleManagementTables.DeleteRolesMenu(roleAccess.roleID);
                    //Insert Data
                    foreach (AccessType accessType in roleAccess.dataaccess)
                    {
                        RoleManagementTables.InsertNewRoleAccess(new RoleMenuAccess { RoleID = roleAccess.roleID, MenuHdrID = accessType.moduleID });
                    }
                    return Json(new { isError = "F", message = "New role has been added!" });
                }
                catch (Exception ex)
                {
                    return Json(new { isError = "T", message = "Could not insert data." });
                }
            }
            else
            {
                return Json(new { isError = "T", message = "Could not insert data." });
            }
        }
    }
}