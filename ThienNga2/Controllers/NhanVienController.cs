using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThienNga2.Models.Entities;

namespace ThienNga2.Controllers
{

    public class NhanVienController : EntitiesAM
    {
        // GET: NhanVien
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewData["SystemUser"] = am.AspNetUsers.ToList();
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Detail(String id) {
            ViewData["thisaccount"] = am.AspNetUsers.Find(id);
            ViewData["Roles"] = am.AspNetRoles.ToList();
            return View("Profile", am.AspNetUsers.Find(id));
        }
        [Authorize]
        public JsonResult getNewNoti() {
            string userid = User.Identity.GetUserId();
            try {
                ThongBaoMoi thongbao = am.ThongBaoMois.Where(u => u.Reciever.Equals(userid) && !u.Confirmed).FirstOrDefault();
                System.Diagnostics.Debug.WriteLine("AASDASDA " + thongbao.id);
                return Json(new { title = thongbao.Title, link = thongbao.hreflink, description = thongbao.Description, id = thongbao.id }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e) {
                System.Diagnostics.Debug.WriteLine("AASDASDA " + User.Identity.GetUserId());
            }
            return null;
        }
        [Authorize]
        public JsonResult XacNhanSuaChua(int id)
        {
            try {
                ThongBaoMoi thongbao = am.ThongBaoMois.Find(id);
                thongbao.Confirmed = true;
                am.SaveChanges();
                return Json(new { result = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) { }
            return Json(new { result = "fail" }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult DeactiveAccount(String id) {
            AspNetUser user =am.AspNetUsers.Find(id);
            user.UserName = user.UserName + "removedAccount";
            am.SaveChanges();
            return RedirectToAction("Detail", new { id = id}) ;
        }
        public ActionResult ReactiveAccount(String id)
        {
            AspNetUser user = am.AspNetUsers.Find(id);
            user.UserName = user.UserName.Replace("removedAccount", "");
            am.SaveChanges();
            return RedirectToAction("Detail", new { id = id });
        }
    }
}