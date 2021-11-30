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

        public (List<CustomerDTO>, decimal TicketExpenseOfTop1, decimal ProductExpenseOfTop1) GetTop5CustomerExpenseByYear(int year)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var cusStatistic = context.Bills.Where(b => b.CreatedAt.Year == year)
                       .GroupBy(b => b.CustomerId)
                       .Select(grC => new
                       {
                           CustomerId = grC.Key,
                           Expense = grC.Sum(c => (Decimal?)(c.TotalPrice + c.DiscountPrice)) ?? 0
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
                           PhoneNumber = cus.PhoneNumber,
                           Expense = statis.Expense
                       }).ToList();

                    decimal TicketExpense = 0, ProductExpense = 0;
                    if (cusStatistic.Count >= 1)
                    {
                        string cusId = cusStatistic.First().Id;
                        TicketExpense = context.Tickets.Where(b => b.Bill.CustomerId == cusId).Sum(t => (decimal?)t.Price) ?? 0;
                        ProductExpense = context.ProductBillInfoes.Where(b => b.Bill.CustomerId == cusId).Sum(t => (decimal?)(t.PricePerItem * t.Quantity)) ?? 0;
                    }
                    return (cusStatistic, Decimal.Truncate(TicketExpense), Decimal.Truncate(ProductExpense));
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public (List<CustomerDTO>, decimal TicketExpenseOfTop1, decimal ProductExpenseOfTop1) GetTop5CustomerExpenseByMonth(int month)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {

                    List<CustomerDTO> cusStatistic;

                    cusStatistic = context.Bills.Where(b => b.CreatedAt.Year == DateTime.Now.Year && b.CreatedAt.Month == month)
                        .GroupBy(b => b.CustomerId)
                        .Select(grC => new
                        {
                            CustomerId = grC.Key,
                            Expense = grC.Sum(c => (Decimal?)(c.TotalPrice + c.DiscountPrice)) ?? 0
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
                            PhoneNumber = cus.PhoneNumber,
                            Expense = statis.Expense

                        }).ToList();

                    decimal TicketExpense = 0, ProductExpense = 0;
                    if (cusStatistic.Count >= 1)
                    {
                        string cusId = cusStatistic.First().Id;
                        TicketExpense = context.Tickets.Where(b => b.Bill.CustomerId == cusId).Sum(t => (decimal?)t.Price) ?? 0;
                        ProductExpense = context.ProductBillInfoes.Where(b => b.Bill.CustomerId == cusId).Sum(t => (decimal?)(t.PricePerItem * t.Quantity)) ?? 0;
                    }
                    return (cusStatistic, Decimal.Truncate(TicketExpense), Decimal.Truncate(ProductExpense));
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion


        #region Staff

        public List<StaffDTO> GetTop5ContributionStaffByYear(int year)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var staffStatistic = context.Bills.Where(b => b.CreatedAt.Year == year)
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
                using (var context = new CinemaManagementEntities())
                {
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
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Movie
        public List<MovieDTO> GetTop5BestMovieByYear(int year)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var movieStatistic = context.Showtimes.Where(s => s.ShowtimeSetting.ShowDate.Year == year)
                    .GroupBy(s => s.MovieId)
                    .Select(gr => new
                    {
                        MovieId = gr.Key,
                        Revenue = gr.Sum(s => (s.Tickets.Count() * s.TicketPrice)),
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
                using (var context = new CinemaManagementEntities())
                {
                    var movieStatistic = context.Showtimes.Where(s => s.ShowtimeSetting.ShowDate.Year == DateTime.Today.Year && s.ShowtimeSetting.ShowDate.Month == month)
                    .GroupBy(s => s.MovieId)
                    .Select(gr => new
                    {
                        MovieId = gr.Key,
                        Revenue = gr.Sum(s => (s.Tickets.Count() * s.TicketPrice)),
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
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Product
        public List<ProductDTO> GetTop5BestProductByYear(int year)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var prodStatistic = context.ProductBillInfoes.Where(b => b.Bill.CreatedAt.Year == year)
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
                using (var context = new CinemaManagementEntities())
                {
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
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

    }
}
