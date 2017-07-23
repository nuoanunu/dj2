using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThienNga2.Models.Entities;

namespace ThienNga2.Models.Service
{
    public class RevenueBusiness
    {
        private ThienNgaDatabaseEntities dbContext;
        public RevenueBusiness(ThienNgaDatabaseEntities DbContext)
        {
            dbContext = DbContext;
        }
        public int GetCustomerThisMonth()
        {
            DateTime now = DateTime.Now;
            try
            {
                int temp = dbContext.tb_customer.Where(u => u.CreatedDate.Month == now.Month && u.CreatedDate.Year == now.Year).Count();
                return temp;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public float GetRevenueLastMonth()
        {
            DateTime now = DateTime.Now;
            now = now.AddMonths(-1);
            float noVATRevenue = 0;
            float vatRevenue = 0;

            try
            {
                noVATRevenue = (float)dbContext.orders.Where(u => u.date.Month == now.Month && u.date.Year == now.Year && u.aftervat == null).Select(u => u.total).DefaultIfEmpty(0).Sum();
            }
            catch (Exception e)
            {

            }
            try
            {
                vatRevenue = (float)dbContext.orders.Where(u => u.date.Month == now.Month && u.date.Year == now.Year && u.aftervat != null).Select(u => u.aftervat).DefaultIfEmpty(0).Sum();
            }
            catch (Exception e)
            {

            }
            return noVATRevenue + vatRevenue;
        }
        public float GetRevenueThisMonth()
        {
            DateTime now = DateTime.Now;
            float noVATRevenue = 0;
            float vatRevenue = 0;
            try
            {
                noVATRevenue = (float)dbContext.orders.Where(u => u.date.Month == now.Month && u.date.Year == now.Year && u.aftervat == null).Select(u => u.total).DefaultIfEmpty(0).Sum();
            }
            catch (Exception e)
            {

            }
            try
            {
                vatRevenue = (float)dbContext.orders.Where(u => u.date.Month == now.Month && u.date.Year == now.Year && u.aftervat != null).Select(u => u.aftervat).DefaultIfEmpty(0).Sum();
            }
            catch (Exception e)
            {

            }
            return noVATRevenue + vatRevenue;
        }
        public int GetFixingItem()
        {
            DateTime now = DateTime.Now;
            try
            {

                int spdangduocsua = dbContext.tb_warranty_activities.Where(u => u.startDate.Month == now.Month && u.startDate.Year == now.Year).Count();
                return spdangduocsua;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public int GetlastMonthItemFixing()
        {
            DateTime now = DateTime.Now;
            now = now.AddMonths(-1);
            try
            {

                int spdangduocsuatt = dbContext.tb_warranty_activities.Where(u => u.startDate.Month == now.Month && u.startDate.Year == now.Year).Count();
                return spdangduocsuatt;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}