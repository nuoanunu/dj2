using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ThienNga2.Models.Entities;
using ThienNga2.Models.ViewModel;
using Microsoft.AspNet.Identity;
using System.Text;
using iTextSharp.text.html.simpleparser;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.UI;
using System.Data;
using System.Globalization;

namespace ThienNga2.Controllers
{
    

    public class WarrantyController : EntitiesAM
    {

        // GET: Warranty

        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public ActionResult Index()
        {
            ViewData["AllWarranty"] = am.tb_warranty_activities.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            AspNetRole role = am.AspNetRoles.SqlQuery("SELECT * FROM AspNetRoles where  id='c58194a2-1502-4623-b549-00cea9250711'").FirstOrDefault();
            List<AspNetUser> nhanviens = role.AspNetUsers.ToList();
           
            ViewData["NhanVienKyThuat"] = nhanviens;
            return View("WarrantyCheck");
        }
        [Authorize(Roles = "Admin,Admin Hà Nội,Nhân Viên Quản Lý Sửa Chữa")]
        public ActionResult GiaoViec(int actid, string empId)
        {
            tb_warranty_activities act= am.tb_warranty_activities.Find(actid);
            AspNetUser user = am.AspNetUsers.Find(empId);
            if (user != null && act != null) {
                act.empFixer = user.Id;
                am.SaveChanges();
            }

            return RedirectToAction("Search", new { code = act.id, searchType= "warrantyActID" });
        }
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public String getTen(String email)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                AspNetUser asp = am.AspNetUsers.SqlQuery("SELECT * FROM AspNetUsers where Email = '" + email + "'").First();
                if (asp != null)
                {
                    emp em = new emp();
                    em.email = email;
                    em.sdt = asp.PhoneNumber;
                    em.fullname = asp.FullName;
                    return serializer.Serialize(em);
                }
            }
            catch (Exception e)
            {

            }
            return "";

        }
        private List<String> allname = new List<String>();
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public ActionResult ConfirmBaoHanh(String idwar, String iduser)
        {
            try
            {
                tb_warranty_activities act = am.tb_warranty_activities.SqlQuery("SELECT * from tb_warranty_activities where CodeBaoHanh= '" + idwar + "'").First();
                AspNetUser user = am.AspNetUsers.SqlQuery("SELECT * FROM AspNetUsers where Email='" + iduser + "'").First();
                
                if (act != null && user != null && User.Identity.GetUserName().Equals(iduser))
                {
                    act.empFixer = user.Id;
                    act.AspNetUser1 = user;
                    am.SaveChanges();
                  
                    return RedirectToAction("Search", "Warranty", new { code = act.id+ "", searchType = "warrantyActID" });
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index", "Warranty");



        }
        public JsonResult updateRole(String updateRole, String userid) {
            AspNetRole rolee = am.AspNetRoles.Find(updateRole);
            AspNetUser user = am.AspNetUsers.Find(userid);
            try {

                if (user != null)
                {
                    AspNetRole role = (AspNetRole)user.AspNetRoles.Where(u => u.Id.Equals(updateRole)).FirstOrDefault();
             
                    if (role != null)
                    {
                        user.AspNetRoles.ToList().Remove(role);
                        am.SaveChanges();
                        return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        user.AspNetRoles.Add(rolee);
                        am.SaveChanges();
                        return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                    }

             

                }
                return Json(new { result = "user null " + userid }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e) {
            }

            return Json(new { result = "fail "  + rolee.Name + " " +user.UserName}, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public void getAllName()
        {
            allname = (List<String>) am.tb_product_detail.Select(u => u.productName);
   
            foreach (String e in allname)
            {
                tb_product_detail t = (tb_product_detail) am.tb_product_detail.Where(u => u.productName.Equals( e ));
                allname.Add(t.productStoreID);
            }
        }

        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public ActionResult IMEILIST() {
           //ViewData["allwar"]=am.tb_warranty.ToList();
           ViewData["allwar"] = am.tb_warranty.SqlQuery("SELECT  * from tb_warranty ").ToList();
           ViewData["dsnkh"] = am.CustomerTypes.ToList();
            return View("allIMEI");
        }

        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public String updateWAR(String wactID, String newDate,String newIMEI, String newSKU, String newName, String newSDT, String newDuration, String newDescription, String newChinhPhu , String newNhomKhach)
        {
            System.Diagnostics.Debug.WriteLine("AAAA");
            try
            {
                tb_warranty wact = am.tb_warranty.Find(int.Parse(wactID));
                if (wact != null) {
                    DateTime date;
               
                    date  = DateTime.ParseExact(newDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);


                    tb_product_detail dt = (tb_product_detail)am.tb_product_detail.Where(u => u.productName.Equals(newSKU) || u.producFactoryID.Equals(newSKU) || u.productStoreID.Equals(newSKU));
                    tb_customer cus = null;
                    if (am.tb_customer.SqlQuery("SELECT * FROM tb_customer where phonenumber='" + newSDT + "'").ToList().Count() >= 1) {
                        cus = am.tb_customer.SqlQuery("SELECT * FROM tb_customer where phonenumber='" + newSDT + "'").First();
                    }
                    int newduration = int.Parse(newDuration);
                    item itt = am.items.Find(wact.item.id);
                
                    if (!wact.item.tb_product_detail.productStoreID.Equals(dt.productStoreID)) {
                        
                        itt.productDetailID = dt.id;
                    }
                    if (wact.duration != newduration) wact.duration = newduration;
                    if (wact.startdate != date) wact.startdate = date;
                    if (wact.description.Equals(newDescription)) wact.description = newDescription;
                    if (!wact.warrantyID.Equals(newIMEI)) wact.warrantyID = newIMEI;
                    if (cus != null)
                    {
                        if (!cus.customerName.Equals(newName)) cus.customerName = newName;
                        am.SaveChanges();

                    }
                    else {
                        cus = new tb_customer();
                        cus.customerName = newName;
                        cus.phonenumber = newSDT;
                        cus.address = "ko co";cus.address2 = "ko co"; cus.Email = "ko co";
                        am.tb_customer.Add(cus);
                        am.SaveChanges();
                        itt.customerID = cus.id;
                    }
                    if (newChinhPhu.Equals("true")) wact.MaChinh = true;
                    else wact.MaChinh = false;
                    wact.item.customerID = cus.id;
                    try
                    {
                        itt.CustomerType = int.Parse(newNhomKhach);
                        cus.Type = int.Parse(newNhomKhach);
                    }
                    catch (Exception e)
                    {

                    }
                    am.SaveChanges();
                }

                checkwarModel model = new checkwarModel();
                model.name = "succeed";
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(model);
            }
            catch (Exception e) {
                System.Diagnostics.Debug.WriteLine("LOI CMNR");
            }
           

            return "";
        }
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public ActionResult Autocomplete(string term)
        {
            allname = (List<String>)am.tb_product_detail.Select(u => u.productName);
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
        // GET: Warranty/Details/5
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Warranty/Create
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public ActionResult Update(String actid, String itemstatus, String day1, String month1, String year1, String day2, String month2, String year2)
        {
            try
            {
                tb_warranty_activities act = am.tb_warranty_activities.Find(int.Parse(actid));
                if (act != null)
                    if (User.Identity.GetUserName().Equals(act.AspNetUser1.Email))
                    {

                        act.status = int.Parse(itemstatus);
                        if (day1 != null && month1 != null && year1 != null)
                        {
                            if (day1.Trim().Length > 0 && month1.Trim().Length > 0 && year1.Trim().Length > 0)
                            {

                                int day = int.Parse(day1);
                                int month = int.Parse(month1);
                                int year = int.Parse(year1);
                                System.Diagnostics.Debug.WriteLine("HERE " + year + " " + month + " " + day);
                                DateTime date = new DateTime(year, month, day);
                        
                                System.Diagnostics.Debug.WriteLine("HERE " + date.ToString());
                                act.realeaseDATE = date;
                            }
                            if (day2 != null && month2 != null && year2 != null)
                            {
                                if (day2.Trim().Length > 0 && month2.Trim().Length > 0 && year2.Trim().Length > 0)
                                {
                                    int day = int.Parse(day2);
                                    int month = int.Parse(month2);
                                    int year = int.Parse(year2);
                                    System.Diagnostics.Debug.WriteLine("HERE " + year + " " + month + " " + day);
                                    DateTime date = new DateTime(year, month, day);
                                 
                                    System.Diagnostics.Debug.WriteLine("HERE " + date.ToString());
                                    act.realeaseDATE = date;
                                }


                            }
                            am.SaveChanges();
                            return RedirectToAction("ActivityDetail", new { id = act.id });

                        }
                    }
            }
            catch (Exception e) { }
            
            return View("WarrantyCheck");

        }
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public double loadPrice(String code, int quantity)
        {

            ViewData["allInvenName"] = am.tb_inventory_name.ToList();
            if (code == null || code.Equals("")) return 0;
            if (code.IndexOf("StoreSKU") > 0)
            {
                code = code.Substring(code.IndexOf("StoreSKU") + 10, code.Length - code.IndexOf("StoreSKU") - 10);
            }
            tb_product_detail t = (tb_product_detail)am.tb_product_detail.Where(u => u.productStoreID.Equals(code));
            if (t == null) return 0;
            else return t.price * quantity;
        }
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public ActionResult UpdateWithFee(tb_warranty_activities item)
        {
            
            return RedirectToAction("Index");

        }

        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]// POST: Warranty/Create
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
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        // GET: Warranty/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        // POST: Warranty/Edit/5
        [HttpPost]

        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]public ActionResult Edit(int id, FormCollection collection)
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
        public ActionResult XoaFee(String feeID, String activitiesID)
        {
            warrantyActivityFee item = am.warrantyActivityFees.Find(int.Parse(feeID));
            if (User.Identity.GetUserName().Equals(item.tb_warranty_activities.AspNetUser1.Email))
            {
                warrantyActivityFee editor = am.warrantyActivityFees.Find(int.Parse(feeID));
                editor.active = false;
                am.SaveChanges();
            }
            System.Diagnostics.Debug.WriteLine("dafug : " + activitiesID);
            return RedirectToAction("ActivityDetail", new { id = activitiesID });

        }
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public ActionResult XoaFixingFee(String feeID, String activitiesID)
        {
            warrantyActivityFixingFee item = am.warrantyActivityFixingFees.Find(int.Parse(feeID));
            if (User.Identity.GetUserName().Equals(item.tb_warranty_activities.AspNetUser1.Email))
            {
                warrantyActivityFixingFee editor = am.warrantyActivityFixingFees.Find(int.Parse(feeID));
                editor.active = false;
                am.SaveChanges();
            }
            return RedirectToAction("ActivityDetail", new { id = activitiesID });

        }
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public ActionResult AddFee(String activitiesID, String ksu, String quantity, String fixPrice)
        {
            if (ksu != null && ksu.Trim().Length > 0)
            {
                tb_product_detail pd = (tb_product_detail)am.tb_product_detail.Where(u => u.productStoreID.Equals(ksu));
                if (pd != null)
                {
                    tb_warranty_activities act = am.tb_warranty_activities.Find(int.Parse(activitiesID));
                    if (act != null)
                    {
                        System.Diagnostics.Debug.WriteLine("aaaaaaaaaaaaa " + act.AspNetUser1.Email + " bbbbbbbb " + User.Identity.GetUserName());
                        if (act.AspNetUser1.Email.Equals(User.Identity.GetUserName()))
                        {
                            System.Diagnostics.Debug.WriteLine("dafug");
                            warrantyActivityFee a = new warrantyActivityFee();
                            a.activityID = int.Parse(activitiesID);
                            a.productSKU = ksu;
                            a.fixingfee = act.tb_warranty.item.tb_product_detail.price * int.Parse(quantity);
                            a.quantity = int.Parse(quantity);
                            am.warrantyActivityFees.Add(a);
                            am.SaveChanges();
                            log log = new log();
                            log.warrantyActivitiesID = act.id;
                            log.account = act.empFixer;
                            log.date = DateTime.Today;
                            log.action = "Da them linh kien moi: " + a.productSKU+  ", so luong: "+a.quantity+"  Tong cong:"+ a.fixingfee.ToString("N");

                            am.logs.Add(log);
                            am.SaveChanges();
                        }
                    }
                }
            }
            return RedirectToAction("ActivityDetail", new { id = activitiesID });
        }
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public ActionResult AllWarranty()
        {
            ViewData["allact"] = am.tb_warranty_activities.ToList();
            return View("WarrantyList");
        }
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public ActionResult AddFixingFee(String fixDetail, String price, String activitiesID)
        {
            tb_warranty_activities act = am.tb_warranty_activities.Find(int.Parse(activitiesID));
            if (fixDetail != null && fixDetail.Trim().Length > 0)
            {


                if (act != null)
                {
                    if (act.AspNetUser1.Email.Equals(User.Identity.GetUserName()))
                    {
                        warrantyActivityFixingFee a = new warrantyActivityFixingFee();
                        a.activityID = int.Parse(activitiesID);
                        a.FixDetail = fixDetail;
                        a.fee = float.Parse(price);
                        am.warrantyActivityFixingFees.Add(a);
                        am.SaveChanges();
                        log log = new log();
                        log.warrantyActivitiesID = act.id;
                        log.account = act.empFixer;
                        log.date = DateTime.Today;
                        log.action = "Them phi sua chua: " + a.fee.ToString("N");

                        am.logs.Add(log);
                        am.SaveChanges();
                    }
                }
            }
            return RedirectToAction("ActivityDetail", new { id = activitiesID });
        }
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public ActionResult ActivityDetail(int id) {
            tb_warranty_activities act = am.tb_warranty_activities.Find(id);
            if (act != null)
            {
                double money = 0;
                foreach (warrantyActivityFee fee in act.warrantyActivityFees) {
                    money = money + fee.fixingfee;
                }
                foreach (warrantyActivityFixingFee fee in act.warrantyActivityFixingFees)
                {
                    money = money + fee.fee;
                }
                ViewData["Total"] = money;
                ViewData["HoaDonBaoHanh"] = act;
                return View("ChiTietHoaDonBaoHanh");
            }
            return RedirectToAction("Index");

        }
   
        // GET: Warranty/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        // POST: Warranty/Delete/5
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
        [Authorize(Roles = "Admin,Nhân Viên kỹ thuật,Bán hàng,Admin Hà Nội")]
        public void GenerateInvoiceBill(String actid)
        {
            float totalheight = 100;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[3] {

                            new DataColumn("Tên sản phẩm/Số lượng", typeof(string)),

                            new DataColumn(" Đơn giá", typeof(string)),

                            new DataColumn("  Thành tiền", typeof(string))});
                tb_warranty_activities act = am.tb_warranty_activities.Find(int.Parse(actid));
                String MaBill = act.CodeBaoHanh;
                String cusname = act.TenKhach;
                String sdt = act.SDT;
                String sp = act.tb_warranty.item.tb_product_detail.productName;
                String mbh = act.tb_warranty.warrantyID;
                String datetake = act.startDate.ToString();
                String des = act.Description;

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StringBuilder sb = new StringBuilder();

                        //Generate Invoice (Bill) Header.
                        sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                        sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Phiếu bảo hành</b></td></tr>");
                        sb.Append("<tr><td colspan = '2'></td></tr>");
                        sb.Append("<tr><td><b>Mã số: </b>");
                        sb.Append(MaBill);
                        sb.Append("</td><td align = 'right'><b>Ngày: </b>");
                        sb.Append(datetake);
                        sb.Append(" </td></tr>");
                        sb.Append("<tr><td colspan = '2'><b>Tên khách: </b>");
                        sb.Append(cusname);
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td colspan = '2'><b>SDT </b>");
                        sb.Append(sdt);
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td colspan = '2'><b>Mã bảo hành: </b>");
                        sb.Append(mbh);
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td colspan = '2'><b>Tên sản phẩm: </b>");
                        sb.Append(sp);
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td colspan = '2'><b>Tình trạng lúc nhận: </b>");
                        sb.Append(des);
                        sb.Append("</td></tr>");
                        sb.Append("</table>");
                        sb.Append("<br />");
                        sb.Append("<table cellpadding='1' >");
                        sb.Append("<tr>");
                        foreach (DataColumn column in dt.Columns)
                        {
                            sb.Append("<th style = 'background-color: #000;color:#ffffff'>");
                            sb.Append(column.ColumnName);
                            sb.Append("</th>");
                        }
                        sb.Append("</tr>");
                        float total = 0;
                        foreach (warrantyActivityFee fee in act.warrantyActivityFees)
                        {
                            bool flag = true;
                            if (fee.active != null) {  if( !(bool)fee.active)  flag = false; }
                            if (flag)
                            {
                                tb_product_detail detail = (tb_product_detail)am.tb_product_detail.Where(u => u.productName.Equals(fee.productSKU));
                                sb.Append("<tr>");
                                sb.Append("<td height='1' colspan='3'><font size='2'>");
                                sb.Append(detail.productName);
                                sb.Append("</font></td>");
                                sb.Append("</tr>");
                                sb.Append("<tr>");
                                sb.Append("<td height='1'><font size='2'>");
                                sb.Append(fee.quantity);
                                sb.Append("</font></td>");
                                sb.Append("<td height='1'><font size='2'>");
                                sb.Append(detail.price);
                                sb.Append("</font></td>");
                                sb.Append("<td height='1'><font size='2'>");
                                sb.Append(Convert.ToDecimal(fee.fixingfee).ToString("#,##0"));
                                sb.Append("</font></td>");
                                sb.Append("</tr>");
                                total = total +  (float)fee.fixingfee;
                                totalheight = totalheight + 15;
                            }
                        }
                        foreach (warrantyActivityFixingFee fee in act.warrantyActivityFixingFees)
                        {
                            bool flag = true;
                            if (fee.active != null) { if (!(bool)fee.active) flag = false; }
                            if (flag)
                            {
                               
                                sb.Append("<tr>");
                                sb.Append("<td height='1' colspan='3'><font size='2'>");
                                sb.Append(fee.FixDetail);
                                sb.Append("</font></td>");
                                sb.Append("</tr>");
                                sb.Append("<tr>");
                                sb.Append("<td height='1'><font size='2'>");
                                sb.Append('1');
                                sb.Append("</font></td>");
                                sb.Append("<td height='1' colspan='2'><font size='2'>");
                                sb.Append(Convert.ToDecimal(fee.fee).ToString("#,##0"));
                                sb.Append("</font></td>");
                             
                                sb.Append("</tr>");
                                total = total + (float)fee.fee;
                                totalheight = totalheight + 15;
                            }
                        }
                        sb.Append("<tr><td align = 'right' colspan = '");
                        sb.Append("1");
                        sb.Append("'>Tong</td>");
                        sb.Append("<td align='right' colspan = '2'>");
                        sb.Append(Convert.ToDecimal(total).ToString("#,##0"));
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");


                        //Export HTML String as PDF.
                        Encoding encoding = Encoding.Unicode;
                        var bytes = encoding.GetBytes(sb.ToString());
                        string str = System.Text.Encoding.Unicode.GetString(bytes);
                        StringReader sr = new StringReader(str);
                        Document pdfDoc = new Document(new Rectangle(Utilities.MillimetersToPoints(78), Utilities.MillimetersToPoints(totalheight)), 0, 0, 0, 0);

                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                        pdfDoc.Open();
                        FontFactory.Register(Server.MapPath("~/fonts/arial-unicode-ms.ttf"), "Arial Unicode MS");
                        StyleSheet style = new StyleSheet();
                        style.LoadTagStyle("body", "face", "Arial Unicode MS");
                        style.LoadTagStyle("body", "encoding", BaseFont.IDENTITY_H);
                        htmlparser.Style = style;
                        htmlparser.StartDocument();
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        Response.ContentEncoding = Encoding.Unicode;
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=PhieuBaoGia_" + MaBill + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.End();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
