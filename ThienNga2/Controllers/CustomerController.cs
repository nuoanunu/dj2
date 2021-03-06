﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThienNga2.Models.Entities;

namespace ThienNga2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerController : EntitiesAM
    {
        // GET: Customer

        public ActionResult Index()
        {
            ViewData["typleList"] = am.CustomerTypes.ToList();
            return View("NhomKhachHang");
        }
        public ActionResult EditGroup(int groupID, String newname, String newcolor, String newchinhsach, String newchinhsach2, String newchinhsach3)
        {
            CustomerType type = am.CustomerTypes.Find(groupID);
            if (type != null)
            {
                type.GroupName = newname;
                type.Color = newcolor;
                type.MoTaChinhSach = newchinhsach + "{.}" + newchinhsach2 + "{.}" +newchinhsach3;
                am.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult CreateGroup(String name, String color, String thongtinbaohanh)
        {
            try
            {
                CustomerType ct = new CustomerType();
                ct.Color = color;
                ct.GroupName = name;
                ct.MoTaChinhSach = thongtinbaohanh;
                am.CustomerTypes.Add(ct);
                am.SaveChanges();
            }
            catch (Exception e)
            {

            }


            ViewData["typleList"] = am.CustomerTypes.ToList();
            return RedirectToAction("Index", "Customer");
        }
        public ActionResult deletethisid(String id)
        {
            try
            {
                am.CustomerTypes.Remove(am.CustomerTypes.Find(int.Parse(id)));
                am.SaveChanges();
            }
            catch (Exception e) { }
            return RedirectToAction("Index");
        }
        public ActionResult DanhSachKhachHang()
        {
            ViewData["dskh"] = am.tb_customer.ToList();
            ViewData["dsnkh"] = am.CustomerTypes.ToList();
            return View("Danhsachkhachhang");
        }
        public ActionResult EditCustomer(int cusID,String customerName, String phonenumber, String address, String address2,  String Email, int Type) {
            tb_customer cus = am.tb_customer.Find(cusID);
            cus.address = address;
            cus.address2 = address2;
            cus.phonenumber = phonenumber;
            cus.customerName = customerName;
            cus.Type = Type;
            cus.Email = Email;
            am.SaveChanges();
            return RedirectToAction("KhachHangDetail" , new { id = cus.id });
        }
        public ActionResult KhachHangDetail(int id)
        {
            ViewData["detail"] = am.tb_customer.Find(id);
            return View("DetailKH");
        }
    }
}