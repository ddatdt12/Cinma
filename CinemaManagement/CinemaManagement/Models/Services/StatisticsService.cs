using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public partial class StatisticsService
    {
        private StatisticsService() { }
        private static StatisticsService _ins;
        public static StatisticsService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new StatisticsService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        #region Customer

        public List<CustomerDTO> GetTop5CustomerExpense()
        {
            try
            {
                var context = DataProvider.Ins.DB;
                var top5Customer = context.Customers.OrderByDescending(
                        c => c.Bills.Sum(b => (Decimal?)b.TotalPrice)
                    ).Take(5).Select(cus => new CustomerDTO
                    {
                        Id = cus.Id,
                        Name = cus.Name,
                        PhoneNumber = cus.PhoneNumber,
                        Email = cus.Email,
                        Expense = cus.Bills.Sum(b => (Decimal?)b.TotalPrice) ?? 0
                    }).ToList();

                return top5Customer;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public (List<CustomerDTO>, decimal TicketExpenseOfTop1 , decimal ProductExpenseOfTop1) GetTop5CustomerExpenseByMonth(int month)
        {
            try
            {
                var context = DataProvider.Ins.DB;

                var cusStatistic = context.Bills.Where(b => b.CreatedAt.Year == DateTime.Now.Year && b.CreatedAt.Month == month)
                    .GroupBy(b => b.CustomerId)
                    .Select(grC => new
                    {
                        CustomerId = grC.Key,
                        Expense = grC.Sum(c=> (Decimal?)(c.TotalPrice + c.DiscountPrice)) ?? 0
                    })
                    .OrderByDescending(b => b.Expense).Take(5)
                    .Join(
                    context.Customers,
                    statis => statis.CustomerId,
                    cus => cus.Id,
                    (statis, cus) => new CustomerDTO
                    {
                        Id = cus.Id,
                        Name = cus.Name,
                        Expense =  statis.Expense
                    }).ToList();

                decimal TicketExpense = 0, ProductExpense = 0;
                if (cusStatistic.Count >= 1)
                {
                     TicketExpense = context.Tickets.Where(b => b.Bill.CustomerId == cusStatistic.First().Id).Sum(t => (decimal?)t.Price) ?? 0;
                     ProductExpense = context.ProductBillInfoes.Where(b => b.Bill.CustomerId == cusStatistic.First().Id).Sum(t => (decimal?)(t.PricePerItem * t.Quantity)) ?? 0;
                }
                return (cusStatistic, TicketExpense, ProductExpense);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion


        #region Staff

        public List<StaffDTO> GetTop5ContributionStaff()
        {
            try
            {
                var context = DataProvider.Ins.DB;
                var top5Staff = context.Staffs
                    .OrderByDescending(
                        c => c.Bills.Sum(b => (Decimal?)b.TotalPrice) // nice trick to prevent bills is empty
                    ).Take(5).Select(s => new StaffDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        BenefitContribution = s.Bills.Sum(b => (Decimal?)b.TotalPrice) ?? 0
                    }).ToList();

                return top5Staff;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<StaffDTO> GetTop5ContributionStaffByMonth(int month)
        {
            try
            {
                var context = DataProvider.Ins.DB;

                var staffStatistic = context.Bills.Where(b => b.CreatedAt.Year == DateTime.Today.Year && b.CreatedAt.Month == month)
                    .GroupBy(b => b.StaffId)
                    .Select(grB => new
                    {
                        StaffId = grB.Key,
                        BenefitContribution = grB.Sum(b => (Decimal?)b.TotalPrice) ?? 0
                    })
                    .OrderByDescending(b => b.BenefitContribution).Take(5)
                    .Join(
                    context.Staffs,
                    statis => statis.StaffId,
                    staff => staff.Id,
                    (statis, staff) => new StaffDTO
                    {
                        Id = staff.Id,
                        Name = staff.Name,
                        BenefitContribution = statis.BenefitContribution
                    }).ToList();

                return staffStatistic;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Movie
        public List<MovieDTO> GetTop5BestMovie()
        {
            try
            {
                var context = DataProvider.Ins.DB;
                var top5Movie = context.Movies.OrderByDescending(
                        m => m.Showtimes.Sum(s => (decimal?)(s.Tickets.Count() * s.TicketPrice))
                        ).Take(5).Select(m =>
                             new MovieDTO
                             {
                                 Id = m.Id,
                                 DisplayName = m.DisplayName,
                                 Revenue = m.Showtimes.Sum(s => (decimal?)(s.Tickets.Count() * s.TicketPrice)) ?? 0,
                                 TicketCount = m.Showtimes.Sum(s => (int?)s.Tickets.Count()) ?? 0,
                             }
                        ).ToList();
                return top5Movie;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<MovieDTO> GetTop5BestMovieByMonth(int month)
        {
            try
            {
                var context = DataProvider.Ins.DB;

                var movieStatistic = context.Showtimes.Where(s => s.ShowtimeSetting.ShowDate.Year == DateTime.Today.Year && s.ShowtimeSetting.ShowDate.Month == month)
                    .GroupBy(s => s.MovieId)
                    .Select(gr => new
                    {
                        MovieId = gr.Key,
                        Revenue = gr.Sum(s => s.Tickets.Count() * s.TicketPrice),
                        TicketCount = gr.Sum(s => s.Tickets.Count())
                    })
                    .OrderByDescending(m => m.Revenue).Take(5)
                    .Join(
                    context.Movies,
                    statis => statis.MovieId,
                    movie => movie.Id,
                    (statis, movie) => new MovieDTO
                    {
                        Id = movie.Id,
                        DisplayName = movie.DisplayName,
                        Revenue = statis.Revenue,
                        TicketCount = statis.TicketCount
                    }).ToList();
                return movieStatistic;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Product
        public List<ProductDTO> GetTop5BestProduct()
        {
            try
            {
                var context = DataProvider.Ins.DB;
                var top5Product = context.Products.OrderByDescending(
                        p => p.ProductBillInfoes.Sum(pDetails => (Decimal?)(pDetails.Quantity * pDetails.PricePerItem))
                        ).Take(5).Select(p =>
                             new ProductDTO
                             {
                                 Id = p.Id,
                                 DisplayName = p.DisplayName,
                                 Revenue = p.ProductBillInfoes.Sum(pBill => (Decimal?)(pBill.Quantity * pBill.PricePerItem)) ?? 0,
                                 SalesQuantity = p.ProductBillInfoes.Sum(pBill => (int?)pBill.Quantity) ?? 0
                             }
                        ).ToList();
                return top5Product;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<ProductDTO> GetTop5BestProductByMonth(int month)
        {
            try
            {
                var context = DataProvider.Ins.DB;

                var prodStatistic = context.ProductBillInfoes.Where(b => b.Bill.CreatedAt.Year == DateTime.Today.Year && b.Bill.CreatedAt.Month == month)
                    .GroupBy(pBill => pBill.ProductId)
                    .Select(gr => new
                    {
                        ProductId = gr.Key,
                        Revenue = gr.Sum(pBill => (Decimal?)(pBill.Quantity * pBill.PricePerItem)) ?? 0,
                        SalesQuantity = gr.Sum(pBill => (int?)pBill.Quantity) ?? 0
                    })
                    .OrderByDescending(m => m.Revenue).Take(5)
                    .Join(
                    context.Products,
                    statis => statis.ProductId,
                    prod => prod.Id,
                    (statis, prod) => new ProductDTO
                    {
                        Id = prod.Id,
                        DisplayName = prod.DisplayName,
                        Revenue = statis.Revenue,
                        SalesQuantity = statis.SalesQuantity
                    }).ToList();
                return prodStatistic;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

    }
}
