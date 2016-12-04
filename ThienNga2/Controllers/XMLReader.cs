using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using ThienNga2.Models.Entities;

namespace ThienNga2.Controllers
{
    public class XMLReader
    {

        public static List<List<tb_product_detail>> reader(XmlDocument doc)
        {
            List<List<tb_product_detail>> result = new List<List<tb_product_detail>>();
            List<tb_product_detail> totaladd = new List<tb_product_detail>();
            List<tb_product_detail> totalupdate = new List<tb_product_detail>();
            XDocument xDoc = XDocument.Load(new XmlNodeReader(doc));
            XElement all = xDoc.Root;
            ThienNgaDatabaseEntities am = new ThienNgaDatabaseEntities();
            int t = 0;
            int t2 = 0;
            foreach (XElement element in all.Descendants("Product"))
            {
                t = t + 1;
                try
                {
                    String Name = element.Element("Name").Value;

                    System.Diagnostics.Debug.WriteLine("READ " + Name);
                    String FullDescription = element.Element("FullDescription").Value;
                    XElement ProductVariants = element.Element("ProductVariants");
                    foreach (XElement element2 in ProductVariants.Descendants("ProductVariant"))
                    {
                        t2 = t2 + 1;
                        if (element2.Element("ManufacturerPartNumber").Value != null || element2.Element("SKU").Value != null)
                        {
                            String ManufacturerPartNumber = element2.Element("ManufacturerPartNumber").Value;
                            String SKU = element2.Element("SKU").Value;

                            if (SKU == null && ManufacturerPartNumber != null)
                                SKU = ManufacturerPartNumber;
                            if (SKU != null && ManufacturerPartNumber != null)
                            {
                                if (SKU.Trim().Length < 1) SKU = ManufacturerPartNumber;
                            }
                            if (ManufacturerPartNumber == null) ManufacturerPartNumber = SKU;
                            else if (ManufacturerPartNumber.Trim().Length < 1) ManufacturerPartNumber = SKU;
                            System.Diagnostics.Debug.WriteLine("SKU " + SKU + "  aa " + ManufacturerPartNumber);
                            if (SKU.Length > 0)
                            {


                                String Price = element2.Element("Price").Value;
                                float pricef = float.Parse(Price) / 10000;

                                tb_product_detail a = new tb_product_detail();
                                a.producFactoryID = ManufacturerPartNumber;
                                a.productStoreID = SKU;
                                a.productName = Name;
                                if (FullDescription != null)
                                    a.description = FullDescription;
                                a.price = pricef;
                                tb_product_detail b = am.tb_product_detail.Where(u => u.productStoreID.Equals(SKU) || u.producFactoryID.Equals(SKU)).FirstOrDefault();
                                if (b != null)
                                {
                                    if (!b.productStoreID.Equals(a.productStoreID) || !b.producFactoryID.Equals(a.producFactoryID) || !b.productName.Equals(a.productName) ||
                                        !b.description.Equals(a.description) || !b.price.Equals(a.price))
                                    {
                                        b.lastUpdate = DateTime.Now;
                                        totalupdate.Add(b);
                                    }
                                    b.productStoreID = a.productStoreID;
                                    b.price = a.price;

                                    b.productName = a.productName;
                                    b.description = a.description;
                                    b.producFactoryID = a.producFactoryID;

                                    am.SaveChanges();
                                }
                                else
                                {
                                    totaladd.Add(b);
                                    a.addedDate = DateTime.Now;
                                    am.tb_product_detail.Add(a);
                                    am.SaveChanges();
                                }


                            }
                        }
                    }
                }
                catch (Exception e) { }

            }
            System.Diagnostics.Debug.WriteLine("t1 " + t + "  aa " + t2);
            result.Add(totaladd);
            result.Add(totalupdate);
            return result;

            return null;
        }
    }

}