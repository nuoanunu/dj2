﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThienNga2.Controllers;
using ThienNga2.Models;
using ThienNga2.Models.Entities;
using ThienNga2.Models.ViewModel;
using Twilio;

namespace ThienNga2.Areas.Admin.Controllers
{
    
    public class InventoryController : EntitiesAM
    {

        private List<String> allname = new List<String>();

        [Authorize(Roles = "Admin,Kiểm Kho")]
        public ActionResult Autocomplete(string term)
        {
            allname = am.ThienNga_FindProductName2("").ToList();
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

        // GET: Admin/Invenotry
        [Authorize(Roles = "Admin,Kiểm Kho")]
        public ActionResult AllInven() {
            ViewData["InvenLisst"] = am.inventories.ToList();
            return View("AllKho");
        }
        [Authorize(Roles = "Admin,Kiểm Kho")]
        public ActionResult Index()
        {

            System.Diagnostics.Debug.WriteLine("SIZE " + allname.Count());
            ViewData["allInvenName"] = am.tb_inventory_name.ToList();
            return View("Inventory");
        }
        [Authorize(Roles = "Admin,Kiểm Kho")]
        // GET: Admin/Search
        public ActionResult Search(string code)
        {
            try
            {
                if (code.Trim().Length > 0)
                {
                    ViewData["allInvenName"] = am.tb_inventory_name.ToList();
                    if (code == null || code.Equals("")) return View("Inventory");
                    if (code.IndexOf("StoreSKU") > 0)
                    {
                        code = code.Substring(code.IndexOf("StoreSKU") + 10, code.Length - code.IndexOf("StoreSKU") - 10);
                    }
                    tb_product_detail t = am.tb_product_detail.Where(u=>u.productStoreID.Equals(code) || u.producFactoryID.Equals(code)).FirstOrDefault();
                    if (t == null) return View("Inventory");
                    ViewData["productdetail"] = t;
                    ViewData["dsspdt"] = am.ThienNga_checkkho2(t.productStoreID).ToList();
                }
            }
            catch (Exception e) {
            }
            
          //  ViewData["dsspdt"] = am.inventories.ToList();
            return View("Inventory");
        }
    
        [Authorize(Roles = "Admin,Kiểm Kho")]
        public ActionResult SearchGetAll( String idd )
        {
            ViewData["allInvenName"] = am.tb_inventory_name.ToList();
            if (idd == null || idd.Equals("")) {
                
                return View("Inventory");

            }
          
         
            int id = int.Parse(idd);
            List<ThienNga_getkho_Result2> lstt = am.ThienNga_getkhoFinal(id).ToList();
            ViewData["invename"] =  am.tb_inventory_name.Find(1).InventoryName;
            ViewData["allInven"] = lstt;
            return View("Inventory");
        }
        [Authorize(Roles = "Admin,Kiểm Kho")]
        [HttpPost]
        public ActionResult Search2(string code, string invID)
        {
            if( code.IndexOf("StoreSKU") > 0 ) {
                code = code.Substring(code.IndexOf("StoreSKU") + 10, code.Length - code.IndexOf("StoreSKU") - 10);
            }
             
            System.Diagnostics.Debug.WriteLine(" no dayu ne " + invID);

            ViewData["productdetail"] = am.tb_product_detail.Where(u => u.productStoreID.Equals(code) || u.producFactoryID.Equals(code)).FirstOrDefault();

            List<inventory> lst = am.ThienNga_checkkho2(code).ToList();
            System.Diagnostics.Debug.WriteLine(" no day ne " + invID + " no day ne" + code + "SIZE NE " + lst.Count() ) ;

            foreach (inventory i in lst) {
                if (i.inventoryID == int.Parse(invID)) ViewData["inventoryDetail"] = i;
            }
            
            //  ViewData["dsspdt"] = am.inventories.ToList();
            return View("NhapKho");
        }
        [Authorize(Roles = "Admin,Kiểm Kho")]
        [HttpPost]
        public ActionResult Search3(string code, string invID)
        {
            if (code.IndexOf("StoreSKU") > 0)
            {
                code = code.Substring(code.IndexOf("StoreSKU") + 10, code.Length - code.IndexOf("StoreSKU") - 10);
            }

            ViewData["productdetail"] = am.tb_product_detail.Where(u => u.productStoreID.Equals(code) || u.producFactoryID.Equals(code)).FirstOrDefault();
            List<inventory> lst = am.ThienNga_checkkho2(code).ToList();
            foreach (inventory i in lst)
            {
                if (i.inventoryID == int.Parse(invID)) ViewData["inventoryDetail"] = i;
            }
            //  ViewData["dsspdt"] = am.inventories.ToList();
            return View("XuatKho");
        }
        [Authorize(Roles = "Admin,Kiểm Kho")]
        // GET: Admin/Invenotry/Details/
        public ActionResult Details(int id)
        {
           
            return View();
        }
        [Authorize(Roles = "Admin,Quản lý kho,Thêm Kho")]
        public ActionResult themkho( )
        {
            List<tb_inventory_name> nameList = am.tb_inventory_name.ToList();

            List<SelectListItem> ls = new List<SelectListItem>();
            SelectList ls2 = new SelectList(ls);
             
            
            foreach (tb_inventory_name y in nameList) {
                ls.Add(new SelectListItem { Text = y.InventoryName, Value = y.id + "" });
            }
            ViewData["invID"] = ls2;
            ViewBag.invID = new SelectList(ls, "Value", "Text");
            return View("Inventory");
        }
        [Authorize(Roles = "Admin,Thêm Kho")]
        public ActionResult addkho(InvenotyChangeModel fixkho)
        {
            inventory t =am.inventories.Find(fixkho.inven.id);
            if (fixkho.newadd <= 0) { }
            else
            {
                t.quantity = t.quantity + fixkho.newadd;
                am.SaveChanges();
            }
            ViewData["productdetail"] = am.tb_product_detail.Where(u=>u.productStoreID.Equals(fixkho.inven.productFactoryCode)).FirstOrDefault();
            ViewData["inventoryDetail"] = am.ThienNga_checkkho2(fixkho.inven.productFactoryCode).ToList();
            return RedirectToAction("Search", "Inventory", new { code = t.productStoreCode+"" });
          
        }
        [Authorize(Roles = "Admin,Thêm Kho")]
        public ActionResult orderKho(InvenotyChangeModel fixkho)
        {
            inventory t = am.inventories.Find(fixkho.inven.id);
            if (fixkho.newadd <= 0) { }
            else
            {
                t.Incoming = t.Incoming + fixkho.newadd;
                DateTime date = new DateTime(fixkho.year , fixkho.month , fixkho.day);
                t.DateOrdered = date;
                am.SaveChanges();
            }
           
            ViewData["productdetail"] = am.tb_product_detail.Where(u => u.productStoreID.Equals(fixkho.inven.productFactoryCode) || u.producFactoryID.Equals(fixkho.inven.productFactoryCode)).FirstOrDefault();
            ViewData["inventoryDetail"] = am.ThienNga_checkkho2(fixkho.inven.productFactoryCode).ToList();
            return RedirectToAction("Search", "Inventory", new { code = t.productStoreCode + "" });

        }
       
        [Authorize(Roles = "Admin,Xuất Kho")]
        public ActionResult removeKho(InvenotyChangeModel fixkho)
        {
            inventory t = am.inventories.Find(fixkho.inven.id);
            if (fixkho.newadd > t.quantity) return RedirectToAction("Search", "Inventory", new { code = t.productStoreCode + "" });
            t.quantity = t.quantity - fixkho.newadd;
            am.SaveChanges();
            ViewData["productdetail"] = am.tb_product_detail.Where(u => u.productStoreID.Equals(fixkho.inven.productFactoryCode) || u.producFactoryID.Equals(fixkho.inven.productFactoryCode)).FirstOrDefault();
            ViewData["inventoryDetail"] = am.ThienNga_checkkho2(fixkho.inven.productFactoryCode).ToList();
            return RedirectToAction("Search", "Inventory", new { code = t.productStoreCode + "" });
        }
        [Authorize(Roles = "Admin,Xuất Kho")]
        public ActionResult changeKho(InvenotyChangeModel fixkho)
        {

            System.Diagnostics.Debug.WriteLine( "  check ID nòa  "  + fixkho.inven.productStoreCode);
            inventory t = am.inventories.Find(fixkho.inven.id);
            int id2 = int.Parse(fixkho.inven.productStoreCode); 
            inventory t2 = am.inventories.Find(id2);
            if(fixkho.newadd > t.quantity) return RedirectToAction("Search", "Inventory", new { code = t.productStoreCode + "" });
            t.quantity = t.quantity - fixkho.newadd;
            t2.quantity = t2.quantity + fixkho.newadd;
            am.SaveChanges();
            ViewData["productdetail"] = am.tb_product_detail.Where(u => u.productStoreID.Equals(fixkho.inven.productFactoryCode) || u.producFactoryID.Equals(fixkho.inven.productFactoryCode)).FirstOrDefault();
            ViewData["inventoryDetail"] = am.ThienNga_checkkho2(fixkho.inven.productFactoryCode).ToList();
            return RedirectToAction("Search", "Inventory", new { code = t.productStoreCode + "" });
        }


        // GET: Admin/Invenotry/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Invenotry/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Invenotry/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Invenotry/Edit/5
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

        // GET: Admin/Invenotry/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Invenotry/Delete/5
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
