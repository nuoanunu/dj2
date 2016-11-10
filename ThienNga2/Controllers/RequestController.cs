using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThienNga2.Models.Entities;
namespace ThienNga2.Controllers
{
    public class RequestController : EntitiesAM
    {
        // GET: Request
        public ActionResult Index()
        {
            ViewData["AllRequest"] = am.RequestMuons.OrderByDescending(u => u.createdDate).ToList();
            return View("RequestControll");
        }
        [Authorize(Roles = "Nhân Viên Quản Lý Sửa Chữa,Admin,Quản lý kho")]
        public ActionResult AllowRequest(int id) {
            RequestMuon req = am.RequestMuons.Find(id);
            if (req != null && req.status == 1) {
                req.status = 2;
                am.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin,Quản lý kho")]
        public ActionResult Given(int id) {
            RequestMuon req = am.RequestMuons.Find(id);
            if (req != null && req.status ==2)
            {
                req.status = 3;
                am.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin,Quản lý kho")]
        public ActionResult returned(int id) {
            RequestMuon req = am.RequestMuons.Find(id);
            if (req != null && req.status == 4)
            {
                req.status = 5;
                am.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}