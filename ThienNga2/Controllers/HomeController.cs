using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThienNga2.Models.Entities;
namespace ThienNga2.Controllers
{
    [Authorize]
    public class HomeController : EntitiesAM
    {
        public ActionResult Index()
        {
            DateTime now = DateTime.Now;
            DateTime lastmonth = now.AddMonths(-1);
            int totalcustomer = am.tb_customer.Count();
            float temp = 1;
            try
            {
                temp = am.tb_customer.Where(u => u.CreatedDate.Month == now.Month && u.CreatedDate.Year == now.Year).Count() / am.tb_customer.Count();
            }
            catch (Exception e) {
                temp = 1;
            }

            float customerincrease = (float)(100f *Math.Floor(temp));
            ViewData["totalcustomer"] = am.tb_customer.Count();
            ViewData["customerincrease"] = (float)customerincrease;
        
                float temptotalrevenue = (float) am.orders.Where(u => u.date.Month == now.Month && u.date.Year == now.Year && u.aftervat == null).Select(u=> u.total).DefaultIfEmpty(0) .Sum( );
                temptotalrevenue = temptotalrevenue + (float)am.orders.Where(u => u.date.Month == now.Month && u.date.Year == now.Year && u.aftervat != null).Select(u => u.aftervat).DefaultIfEmpty(0).Sum();
                float temptotalrevenuelastmonth = (float)am.orders.Where(u => u.date.Month == lastmonth.Month && u.date.Year == lastmonth.Year && u.aftervat == null).Sum(u => u.total);
                temptotalrevenuelastmonth = temptotalrevenuelastmonth + (float)am.orders.Where(u => u.date.Month == lastmonth.Month && u.date.Year == lastmonth.Year && u.aftervat != null && u.aftervat > 0).Select(u => u.aftervat).DefaultIfEmpty(0).Sum();
                ViewData["Revenue"] = temptotalrevenue;
                if (temptotalrevenuelastmonth == 0)
                {
                    ViewData["RevenueIncrease"] = (float)100;

                }
                else
                {
                    try
                    {


                        ViewData["RevenueIncrease"] = 100 * (temptotalrevenue - temptotalrevenuelastmonth) / temptotalrevenuelastmonth;
                    }
                    catch (Exception e)
                    {
                        ViewData["RevenueIncrease"] = 100f;
                    }
                }
            
           

            int spdangduocsua = am.tb_warranty_activities.Where(u=>u.startDate.Month == now.Month && u.startDate.Year == now.Year).Count();
            int spdangduocsuatt = am.tb_warranty_activities.Where(u => u.startDate.Month == lastmonth.Month && u.startDate.Year == lastmonth.Year).Count();
            ViewData["SoSanPhamBaohanh"] = spdangduocsua;
            if (spdangduocsuatt == 0) { ViewData["SoSanPhamBaohanhInc"] = (float)100f; }
            else
            {
             
                ViewData["SoSanPhamBaohanhInc"] = (float) 100 * (spdangduocsua - spdangduocsuatt) / spdangduocsuatt;
                
            }


            double chiphisuachua = am.warrantyActivityFees.Where(u => u.tb_warranty_activities.startDate.Month == now.Month && u.tb_warranty_activities.startDate.Year == now.Year && u.fixingfee != null).Select(u => u.fixingfee).DefaultIfEmpty(0).Sum();
            chiphisuachua = chiphisuachua+ am.warrantyActivityFixingFees.Where(u => u.tb_warranty_activities.startDate.Month == now.Month && u.tb_warranty_activities.startDate.Year == now.Year && u.fee != null).Select(u=>u.fee).DefaultIfEmpty(0).Sum();
            System.Diagnostics.Debug.WriteLine("CzxdczxcC " + chiphisuachua);
            if (chiphisuachua == 0) {
                ViewData["chiphisuachua"] = 0f;
                ViewData["chiphisuachuainc"] = -100f;
            }
            else
            {
                try
                {
                    double chiphisuachuatt = am.warrantyActivityFees.Where(u => u.tb_warranty_activities.startDate.Month == lastmonth.Month && u.tb_warranty_activities.startDate.Year == lastmonth.Year && u.fixingfee != null).Select(u => u.fixingfee).DefaultIfEmpty(0).Sum();
                chiphisuachuatt = chiphisuachuatt + am.warrantyActivityFixingFees.Where(u => u.tb_warranty_activities.startDate.Month == lastmonth.Month && u.tb_warranty_activities.startDate.Year == lastmonth.Year && u.fee != null).Select(u => u.fee).DefaultIfEmpty(0).Sum();
               
                    if (chiphisuachuatt == 0)
                    {
                        ViewData["chiphisuachuainc"] = (float)100;

                    }
                    else
                    {
                        ViewData["chiphisuachuainc"] = (float)(100 * (chiphisuachua - chiphisuachuatt) / chiphisuachuatt);

                    }
                }
                catch (Exception e) {
                    ViewData["chiphisuachuainc"] = -100f;
                }
            
                ViewData["chiphisuachua"] = chiphisuachua;
            }
            List<tb_product_detail> top5fix = am.tb_product_detail.OrderByDescending(u => u.tb_warranty_activities.Where(uu => uu.startDate.Year == now.Year && uu.startDate.Month == now.Month).Count()).Take(5).ToList();
            List<tb_product_detail> top5sell = am.tb_product_detail.OrderByDescending(u => u.items.Where(uu=>uu.order.date.Year==now.Year && uu.order.date.Month == now.Month).Count()).Take(4).ToList();
            List<topsell> top4sold = new List<topsell>();
            foreach (tb_product_detail tb in top5sell) {
                topsell ts = new topsell();
                ts.SKU = tb.productStoreID;
                ts.itemsold = am.items.Where(uu => uu.order.date.Year == now.Year && uu.order.date.Month == now.Month && uu.productDetailID== tb.id).Count();
                top4sold.Add(ts);
            }
            ViewData["top5sell"] = top4sold;
            System.Diagnostics.Debug.WriteLine("ok ngon r mà");
            ViewData["totalsell"] = am.items.Where(uu => uu.order.date.Year == now.Year && uu.order.date.Month == now.Month).Count();
            ViewData["top5fix"] = top5fix;
            ViewData["allorder"] = am.orders.Where(u => u.date.Year == now.Year).ToList();
            ViewData["warrantyActivityFee"] = am.warrantyActivityFees.Where(u => u.tb_warranty_activities.startDate.Year == now.Year).ToList();
            ViewData["warrantyActivityFixingFee"] = am.warrantyActivityFixingFees.Where(u => u.tb_warranty_activities.startDate.Year == now.Year).ToList();

            ViewData["TotalFix"] = am.tb_warranty_activities.Count();
            List<AspNetUser> top5active = am.AspNetUsers.OrderByDescending(u => u.tb_warranty_activities1.Count()).Take(5).ToList();
            int totaldiem = top5active.Sum(u => u.tb_warranty_activities1.Count());
            if (totaldiem > 0)
                ViewData["totaldiem"] = totaldiem;
            else {
                ViewData["totaldiem"] = 1;
            }
            ViewData["top5active"] = top5active;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       
    }
}