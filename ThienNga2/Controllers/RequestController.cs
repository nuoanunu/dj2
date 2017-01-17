using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThienNga2.Models.Entities;
using Microsoft.AspNet.Identity;
namespace ThienNga2.Controllers
{
    public class RequestController : EntitiesAM
    {
        // GET: Request
        [Authorize(Roles = "Admin,Quản lý kho,Nhân Viên kỹ thuật,Nhân Viên Quản Lý Sửa Chữa")]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            if (!User.IsInRole("Nhân Viên Quản Lý Sửa Chữa") && !User.IsInRole("Quản lý kho") && !User.IsInRole("Admin"))
            {
                ViewData["AllRequest"] = am.RequestMuons.Where(u => u.creator.Equals(userId)).OrderByDescending(u => u.createdDate).ToList();
            }
            else {
                ViewData["AllRequest"] = am.RequestMuons.OrderByDescending(u => u.createdDate).ToList();
            }
            return View("RequestControll");
        }
    
        [Authorize(Roles = "Admin,Quản lý kho,Nhân Viên Quản Lý Sửa Chữa")]
        public ActionResult AllowRequest(int id) {
            RequestMuon req = am.RequestMuons.Find(id);
            if (req != null && req.status ==1)
            {
                req.status = 3;
                am.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin,Quản lý kho,Nhân Viên Quản Lý Sửa Chữa")]
        public ActionResult returned(int id) {
            RequestMuon req = am.RequestMuons.Find(id);
            if (req != null && req.status == 3)
            {
                req.status = 5;
                am.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin,Quản lý kho,Nhân Viên Quản Lý Sửa Chữa")]
        public ActionResult DeleteRequest(int id) {
            RequestMuon req = am.RequestMuons.Find(id);
            if (req != null && req.status == 1)
            {
                am.RequestMuons.Remove(req);
                am.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}