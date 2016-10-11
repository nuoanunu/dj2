using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThienNga2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NhanVienController : EntitiesAM
    {
        // GET: NhanVien
        public ActionResult Index()
        {
            ViewData["SystemUser"] = am.AspNetUsers.ToList();
            return View();
        }
        public ActionResult Detail(String id) {
            ViewData["thisaccount"] = am.AspNetUsers.Find(id);
            ViewData["Roles"] = am.AspNetRoles.ToList();
            return View("Profile");
        }
    }
}