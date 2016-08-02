﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThienNga2.Models.Entities;

namespace ThienNga2.Controllers
{
    public class CreateWarrantyController : Controller
    {
        private ThienNgaDatabaseEntities am = new ThienNgaDatabaseEntities();
        private List<String> allname = new List<String>();
        public void getAllName()
        {
           List<AspNetUser> lst = am.AspNetUsers.ToList();
            foreach (AspNetUser au in lst) {
                allname.Add(au.Email);
            }
        }
        public ActionResult Autocomplete(string term)
        {
            getAllName();
            System.Diagnostics.Debug.WriteLine("SIZE " + allname.Count());

            List<String> result = new List<string>();
            foreach (String e in allname)
            {
                if (e.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    result.Add(e);
                }
            }
            //  return Json(result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: CreateWarranty
        public ActionResult Index()
        {
            return View("CreateWarranty");
        }

        // GET: CreateWarranty/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CreateWarranty/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreateWarranty/CreateNew
        [HttpPost]
        public ActionResult CreateNew(String actid, String phoneNumber ,String cusname, String IMEI, String Descrip, String Emp1, String Emp2)
        {
            tb_warranty_activities act;
            if (actid != null ) {
                if (actid.Trim().Length > 0) {
                    try
                    {
                        act = am.tb_warranty_activities.Find(int.Parse(actid));
                    }
                    catch { return View("CreateWarranty"); }
                }
                else act = new tb_warranty_activities();

            }
            else
                     act = new tb_warranty_activities();
            
               
                phoneNumber = phoneNumber.Trim();
                var emplo1 = am.AspNetUsers.SqlQuery("SELECT * FROM dbo.AspNetUsers WHERE Email='" + Emp1 + "'").ToList().First();
                var emplo2 = am.AspNetUsers.SqlQuery("SELECT * FROM dbo.AspNetUsers WHERE Email='" + Emp2 + "'").ToList().First();
                act.employee = (string)emplo1.Id;
                act.empFixer = (string)emplo2.Id;
                act.TenKhach = cusname;
                act.SDT = phoneNumber;
                act.status = 1;
                act.Description = Descrip;
                act.warrantyID = IMEI;
                act.startDate = DateTime.Today;
                String date = act.startDate.Day.ToString();
                if (date.Length == 1) date = "0" + date;
                String month = act.startDate.Month.ToString();
                 if (month.Length == 1) month = "0" + month;
                act.CodeBaoHanh =  date + month +"-"+ phoneNumber.Substring(phoneNumber.Length - 5);
                    am.tb_warranty_activities.Add(act);
                am.SaveChanges();
                int id = act.id;

            act.id = id;
            ViewData["newwarranty"] = act;
                return View("ConfirmCreate");
       
        }
        [HttpPost]
        public ActionResult EditAct(String actid)
        {
            tb_warranty_activities a = am.tb_warranty_activities.Find(int.Parse(actid));
            ViewData["newwarranty"] = a;
        
            return View("CreateWarranty");
        }
        [HttpPost]
        public ActionResult Confirrm(String actid)
        {
           
            return RedirectToAction("Search","Warranty", new { code = actid, searchType = "warrantyActID" });
        }
        // GET: CreateWarranty/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CreateWarranty/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CreateWarranty/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CreateWarranty/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}