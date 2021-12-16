using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public partial class OverviewStatisticService
    {
        private OverviewStatisticService() { }
        private static OverviewStatisticService _ins;
        public static OverviewStatisticService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new OverviewStatisticService();
                }
                return _ins;
            }
            private set => _ins = value;
        }

        #region Overview
        public async Task<int> GetBillQuantity(int year, int month = 0)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    if (month == 0)
                    {
                        return await context.Bills.Where(b => b.CreatedAt.Year == year).CountAsync();
                    }
                    else
                    {
                        return await context.Bills.Where(b => b.CreatedAt.Year == year && b.CreatedAt.Month == month).CountAsync();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Revenue

        public async Task<(List<decimal>, decimal ProductRevenue, decimal TicketRevenue, string TicketRateStr)> GetRevenueByYear(int year)
        {
            List<decimal> MonthlyRevenueList = new List<decimal>(new decimal[12]);

            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var billList = context.Bills
                    .Where(b => b.CreatedAt.Year == year);

                    (decimal ProductRevenue, decimal TicketRevenue) = await GetFullRevenue(context, year);

                    var MonthlyRevenue = billList
                             .GroupBy(b => b.CreatedAt.Month)
                             .Select(gr => new
                             {
                                 Month = gr.Key,
                                 Income = gr.Sum(b => (decimal?)b.TotalPrice) ?? 0
                             }).ToList();

                    foreach (var re in MonthlyRevenue)
                    {
                        MonthlyRevenueList[re.Month - 1] = decimal.Truncate(re.Income);
                    }

                    (decimal lastProdReve, decimal lastTicketReve) = await GetFullRevenue(context, year - 1);
                    decimal lastRevenueTotal = lastProdReve + lastTicketReve;
                    string RevenueRateStr;
                    if (lastRevenueTotal == 0)
                    {
                        RevenueRateStr = "-2";
                    }
                    else
                    {
                        RevenueRateStr = Helper.ConvertDoubleToPercentageStr((double)((ProductRevenue + TicketRevenue) / lastRevenueTotal) - 1);
                    }

                    return (MonthlyRevenueList, decimal.Truncate(ProductRevenue), decimal.Truncate(TicketRevenue), RevenueRateStr);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<(List<decimal>, decimal ProductRevenue, decimal TicketRevenue, string RevenueRate)> GetRevenueByMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            List<decimal> DailyReveList = new List<decimal>(new decimal[days]);

            try
            {

                using (var context = new CinemaManagementEntities())
                {
                    var billList = context.Bills
                     .Where(b => b.CreatedAt.Year == year && b.CreatedAt.Month == month);

                    (decimal ProductRevenue, decimal TicketRevenue) = await GetFullRevenue(context, year, month);

                    var dailyRevenue = await billList
                                .GroupBy(b => b.CreatedAt.Day)
                                 .Select(gr => new
                                 {
                                     Day = gr.Key,
                                     Income = gr.Sum(b => b.TotalPrice),
                                     DiscountPrice = gr.Sum(b => (decimal?)b.DiscountPrice) ?? 0,
                                 }).ToListAsync();

                    foreach (var re in dailyRevenue)
                    {
                        DailyReveList[re.Day - 1] = decimal.Truncate(re.Income);
                    }

                    if (month == 1)
                    {
                        year--;
                        month = 13;
                    }
                    (decimal lastProdReve, decimal lastTicketReve) = await GetFullRevenue(context, year, month - 1);
                    decimal lastRevenueTotal = lastProdReve + lastTicketReve;
                    string RevenueRateStr;
                    if (lastRevenueTotal == 0)
                    {
                        RevenueRateStr = "-2";
                    }
                    else
                    {
                        RevenueRateStr = Helper.ConvertDoubleToPercentageStr((double)((ProductRevenue + TicketRevenue) / lastRevenueTotal) - 1);
                    }

                    return (DailyReveList, decimal.Truncate(ProductRevenue), decimal.Truncate(TicketRevenue), RevenueRateStr);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Lấy doanh thu của sản phẩm và vé, truyền 1 tham số thì đó sẽ là tìm theo năm, 2 tham số là theo năm và tháng
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<(decimal, decimal)> GetFullRevenue(CinemaManagementEntities context, int year, int month = 0)
        {
            try
            {

                if (month != 0)
                {

                    decimal ProductRevenue = await context.ProductBillInfoes.Where(pB => pB.Bill.CreatedAt.Year == year && pB.Bill.CreatedAt.Month == month)
                                                    .SumAsync(pB => (decimal?)(pB.PricePerItem * pB.Quantity)) ?? 0;
                    decimal TicketRevenue = await context.Tickets.Where(t => t.Bill.CreatedAt.Year == year && t.Bill.CreatedAt.Month == month)
                                                    .SumAsync(t => (decimal?)t.Price) ?? 0;

                    return (ProductRevenue, TicketRevenue);

                }
                else
                {
                    decimal ProductRevenue = await context.ProductBillInfoes.Where(pB => pB.Bill.CreatedAt.Year == year)
                                                    .SumAsync(pB => (decimal?)(pB.PricePerItem * pB.Quantity)) ?? 0;
                    decimal TicketRevenue = await context.Tickets.Where(t => t.Bill.CreatedAt.Year == year)
                                                    .SumAsync(t => (decimal?)t.Price) ?? 0;
                    return (ProductRevenue, TicketRevenue);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion



        #region Expense
        private async Task<decimal> GetFullExpenseLastTime(CinemaManagementEntities context, int year, int month = 0)
        {
            try
            {
                if (month == 0)
                {
                    //Product Receipt
                    decimal  LastYearProdExpense = await context.ProductReceipts
                             .Where(pr => pr.CreatedAt.Year == year)
                             .SumAsync(pr => (decimal?)pr.ImportPrice) ?? 0;

                    //Repair Cost
                    var LastYearRepairCost = await context.Troubles
                             .Where(tr => tr.FinishDate != null && tr.FinishDate.Value.Year == year)
                             .SumAsync(tr => (decimal?)tr.RepairCost) ?? 0;
                    return (LastYearProdExpense + LastYearRepairCost);
                }
                else
                {
                    //Product Receipt
                    decimal LastMonthProdExpense = await context.ProductReceipts
                             .Where(pr => pr.CreatedAt.Year == year && pr.CreatedAt.Month == month)
                             .SumAsync(pr => (decimal?)pr.ImportPrice) ?? 0;
                    //Repair Cost
                    var LastMonthRepairCost = await context.Troubles
                             .Where(tr => tr.FinishDate != null && tr.FinishDate.Value.Year == year && tr.FinishDate.Value.Month == month)
                             .SumAsync(tr => (decimal?)tr.RepairCost) ?? 0;
                    return (LastMonthProdExpense + LastMonthRepairCost);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<(List<decimal> MonthlyExpense, decimal ProductExpense, decimal RepairCost, string ExpenseRate)> GetExpenseByYear(int year)
        {
            List<decimal> MonthlyExpense = new List<decimal>(new decimal[12]);
            decimal ProductExpenseTotal = 0;
            decimal RepairCostTotal = 0;

            //Product Receipt
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var MonthlyProdExpense =await context.ProductReceipts
                     .Where(b => b.CreatedAt.Year == year)
                     .GroupBy(b => b.CreatedAt.Month)
                     .Select(gr => new
                     {
                         Month = gr.Key,
                         Outcome = gr.Sum(b => (decimal?)b.ImportPrice) ?? 0
                     }).ToListAsync();

                    //Repair Cost
                    //var MonthlyRepairCost = MonthlyProdExpense.Select(p => new { Month = p.Month, Outcome = p.Outcome * 2 }).ToList();
                   var MonthlyRepairCost =  await context.Troubles
                        .Where(t => t.FinishDate != null && t.FinishDate.Value.Year == year)
                        .GroupBy(t => t.FinishDate.Value.Month)
                        .Select(gr =>
                        new
                        {
                            Month = gr.Key,
                            Outcome = gr.Sum(t => (decimal?)t.RepairCost) ?? 0
                        }).ToListAsync();


                    //Accumulate
                    foreach (var ex in MonthlyProdExpense)
                    {
                        MonthlyExpense[ex.Month - 1] += decimal.Truncate(ex.Outcome);
                        ProductExpenseTotal += ex.Outcome;
                    }

                    foreach (var ex in MonthlyRepairCost)
                    {
                        MonthlyExpense[ex.Month - 1] += decimal.Truncate(ex.Outcome);
                        RepairCostTotal += ex.Outcome;
                    }

                    decimal lastProductExpenseTotal = await GetFullExpenseLastTime(context, year - 1);

                    string ExpenseRateStr;
                    //check mẫu  = 0
                    if (lastProductExpenseTotal == 0)
                    {
                        ExpenseRateStr = "-2";
                    }
                    else
                    {
                        ExpenseRateStr = Helper.ConvertDoubleToPercentageStr(((double)(ProductExpenseTotal / lastProductExpenseTotal) - 1));
                    }


                    return (MonthlyExpense, decimal.Truncate(ProductExpenseTotal), decimal.Truncate(RepairCostTotal), ExpenseRateStr);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<(List<decimal> DailyExpense, decimal ProductExpense, decimal RepairCost, string RepairRateStr)> GetExpenseByMonth(int year, int month)
        {

            int days = DateTime.DaysInMonth(year, month);
            List<decimal> DailyExpense = new List<decimal>(new decimal[days]);
            decimal ProductExpenseTotal = 0;
            decimal RepairCostTotal = 0;

            try
            {

                using (var context = new CinemaManagementEntities())
                {
                    //Product Receipt
                    var MonthlyProdExpense = await context.ProductReceipts
                         .Where(b => b.CreatedAt.Year == year && b.CreatedAt.Month == month)
                         .GroupBy(b => b.CreatedAt.Day)
                         .Select(gr => new
                         {
                             Day = gr.Key,
                             Outcome = gr.Sum(b => (decimal?)b.ImportPrice) ?? 0
                         }).ToListAsync();
                    //Repair Cost
                    var MonthlyRepairCost = await context.Troubles
                        .Where(t => t.FinishDate != null && t.FinishDate.Value.Year == year && t.FinishDate.Value.Month == month)
                        .GroupBy(t => t.FinishDate.Value.Day)
                        .Select(gr =>
                        new
                        {
                            Day = gr.Key,
                            Outcome = gr.Sum(t => (decimal?)t.RepairCost) ?? 0
                        }).ToListAsync();
                    //context.
                    //Accumulate
                    foreach (var ex in MonthlyProdExpense)
                    {
                        DailyExpense[ex.Day - 1] += decimal.Truncate(ex.Outcome);
                        ProductExpenseTotal += ex.Outcome;
                    }

                    foreach (var ex in MonthlyRepairCost)
                    {
                        DailyExpense[ex.Day - 1] += decimal.Truncate(ex.Outcome);
                        RepairCostTotal += ex.Outcome;
                    }
                    if (month == 1)
                    {
                        year--;
                        month = 13;
                    }


                    decimal lastProductExpenseTotal = await GetFullExpenseLastTime(context, year, month - 1);
                    string ExpenseRateStr;
                    //check mẫu  = 0
                    if (lastProductExpenseTotal == 0)
                    {
                        ExpenseRateStr = "-2";
                    }
                    else
                    {
                        ExpenseRateStr = Helper.ConvertDoubleToPercentageStr(((double)(ProductExpenseTotal / lastProductExpenseTotal) - 1));
                    }

                    return (DailyExpense, ProductExpenseTotal, RepairCostTotal, ExpenseRateStr);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        //public static T GetPropertyValue<T>(object obj, string propName)
        //{
        //    return (T)obj.GetType().GetProperty(propName).GetValue(obj, null);
        //}
    }
}
