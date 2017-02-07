using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ThienNga2.Models.Entities;
using ThienNga2.Models.ViewModel;

namespace ThienNga2.Controllers
{
    [Authorize]
    public class ProductController : EntitiesAM
    {


        // GET: Product

        //public ActionResult Dafug()
        //{

        //    List<tb_product_detail> dlst = am.tb_product_detail.ToList();
        //    List<temp> tlst = am.temps.ToList();
        //    int ty = 0;
        //    String[] rex1 = new string[] { " " };
        //    foreach (temp t in tlst)
        //    {
        //        ty = ty + 1;
        //        try
        //        {
        //            if (t.sdt == null) t.sdt = "DayLaSoGia" + ty;
        //            tb_customer cuss;
        //             cuss = am.tb_customer.SqlQuery("SELECT * from tb_customer where phonenumber='" + t.sdt + "'").First();
        //            if (cuss == null) {
        //                cuss = new tb_customer();
        //                cuss.phonenumber = t.sdt;
        //                if (t.name == null) t.name = "KoCoTen";
        //                cuss.customerName = t.name;
        //                cuss.Type = 4;
        //                cuss.address = "";
        //                cuss.address2 = "";
        //                cuss.Email = "";
        //                am.tb_customer.Add(cuss);
        //                am.SaveChanges();
        //            }
        //            t.date = t.date.Trim();
        //            if(t.series != null)
        //            t.series = t.series.Trim();
        //            if (t.series != null)
        //                if (t.series.Length > 0)
        //            {
        //                String[] rows = t.series.Split(rex1, StringSplitOptions.None);
        //                DateTime date = DateTime.ParseExact(t.date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //                System.Diagnostics.Debug.WriteLine("da heo " + date.ToString() + " cus  id " + cuss.id);
        //                item ite = new item();
        //                ite.productDetailID = 481;
        //                if (t.code != null) {
        //                    if (t.code.Trim().Length > 0) {
        //                        tb_product_detail dt = am.tb_product_detail.SqlQuery("SELECT * FROM tb_product_detail where productStoreID='" +t.code.Trim() + "'").FirstOrDefault();
        //                        if (dt != null) ite.productDetailID = dt.id;
        //                    }
        //                }          ite.customerID = cuss.id;
        //                ite.DateOfSold = date;
        //                ite.orderID = 117;

        //                ite.inventoryID = 1;
        //                ite.productID = "TEMP" + ty;

        //                am.items.Add(ite);
        //                am.SaveChanges();
        //                System.Diagnostics.Debug.WriteLine("da heo " + ite.id);
        //                bool first = true;
        //                foreach (String ttt in rows)
        //                {
        //                        try
        //                        {
        //                            if (ttt != null)
        //                                if (ttt.Trim().Length > 4)
        //                                {


        //                                    if (am.tb_warranty.SqlQuery("SELECT * FROM tb_warranty WHERE warrantyID='" + ttt + "'").ToList().Count() < 1)
        //                                    {

        //                                        tb_warranty newar = new tb_warranty();
        //                                        newar.warrantyID = ttt.Trim();
        //                                        if (first)
        //                                        {
        //                                            first = false;
        //                                            newar.MaChinh = true;
        //                                        }
        //                                        else newar.MaChinh = false;
        //                                        newar.startdate = date;
        //                                        if (t.fakename == null) t.fakename = "Ko co du lieu";
        //                                        newar.description = t.fakename;
        //                                        newar.itemID = ite.id;
        //                                        newar.duration = 24;

        //                                        am.tb_warranty.Add(newar);

        //                                        am.SaveChanges();
        //                                    }
        //                                }
        //                        }
        //                        catch (Exception e) { }
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //        }


        //    }


        //    return View();
        //}
        public ActionResult SearchGetAll( String idd )
        {
            ViewData["allInvenName"] = am.tb_inventory_name.ToList();
            if (idd == null || idd.Equals("")) {
                
                return View("XemThongTin");

            }
          
         
            int id = int.Parse(idd);
            List<ThienNga_getkho_Result2> lstt = am.ThienNga_getkhoFinal(id).ToList();
            ViewData["invename"] =  am.tb_inventory_name.Find(1).InventoryName;
            ViewData["allInven"] = lstt;
            return View("XemThongTin");
        }
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
        private List<String> allname = new List<String>();
        public ActionResult Index()
        {

   
            ViewData["allInvenName"] = am.tb_inventory_name.ToList();
            return View("XemThongTin");
        }
        public ActionResult Search(string code)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Code ne : " + code);
                if (code.Trim().Length > 0)
                {
                    ViewData["allInvenName"] = am.tb_inventory_name.ToList();
                    if (code == null || code.Equals("")) return View("Inventory");
                
                    tb_product_detail t = am.tb_product_detail.Where(u=>u.producFactoryID.Equals(code) || u.productStoreID.Equals(code) || u.productName.Equals(code)) .FirstOrDefault();
                    if (t == null) {
                        ViewData["allInvenName"] = am.tb_inventory_name.ToList();
                        return View("XemThongTin");
                    } 
                    ViewData["productdetail"] = am.tb_product_detail.Where(u => u.producFactoryID.Equals(code) || u.productStoreID.Equals(code)).FirstOrDefault();
                    ViewData["dsspdt"] = am.ThienNga_checkkho2(t.productStoreID).ToList();
                }
            }
            catch (Exception e)
            {
            }

            //  ViewData["dsspdt"] = am.inventories.ToList();
            return View("XemThongTin");
        }
        public List<String> getAll()
        {

            List<String> lst = new List<String>();
            lst.Add("CCC");
            lst.Add("ZZZ");
            foreach (tb_product_detail t in am.tb_product_detail.ToList())
            {
                lst.Add(t.producFactoryID);
                lst.Add(t.productName);
                lst.Add(t.productStoreID);
            }
            return lst;
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
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

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
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

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
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
