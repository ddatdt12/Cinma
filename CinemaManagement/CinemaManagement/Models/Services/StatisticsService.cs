using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public async Task<(List<CustomerDTO>, decimal TicketExpenseOfTop1, decimal ProductExpenseOfTop1)> GetTop5CustomerExpenseByYear(int year)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var cusStatistic = await context.Bills.Where(b => b.CreatedAt.Year == year && b.CustomerId != null)
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
                       }).ToListAsync();

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

        public async Task<(int NewCustomerQuanity, int TotalCustomerQuantity, int WalkinGuestQuantity)> GetDetailedCustomerStatistics(int year, int month = 0)
        {
            try
            {
                if (month == 0)
                {
                    using (var context = new CinemaManagementEntities())
                    {
                        int NewCustomerQuanity = await context.Customers.CountAsync(c => c.CreatedAt.Year == year );
                        int TotalCustomerQuantity = await context.Customers.CountAsync(c => c.Bills.Any(b => b.CreatedAt.Year == year) || c.CreatedAt.Year == year);
                        int WalkinGuestQuantity = await context.Bills.Where(b => b.CustomerId == null && b.CreatedAt.Year == year).CountAsync();
                        return (NewCustomerQuanity, TotalCustomerQuantity, WalkinGuestQuantity);
                    }
                }
                else
                {
                    using (var context = new CinemaManagementEntities())
                    {
                        int NewCustomerQuanity = await context.Customers.CountAsync(c => c.CreatedAt.Year == year && c.CreatedAt.Month == month);
                        int TotalCustomerQuantity = await context.Customers
                            .CountAsync(c => c.Bills.Any(b => b.CreatedAt.Year == year && b.CreatedAt.Month == month) || (c.CreatedAt.Year == year && c.CreatedAt.Month == month ));
                        int WalkinGuestQuantity = await context.Bills.Where(b => b.CustomerId == null && b.CreatedAt.Year == year && b.CreatedAt.Month == month).CountAsync();
                        return (NewCustomerQuanity, TotalCustomerQuantity, WalkinGuestQuantity);
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<(List<CustomerDTO>, decimal TicketExpenseOfTop1, decimal ProductExpenseOfTop1)> GetTop5CustomerExpenseByMonth(int month)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    List<CustomerDTO> cusStatistic = await context.Bills.Where(b => b.CreatedAt.Year == DateTime.Now.Year && b.CreatedAt.Month == month && b.CustomerId != null)
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

                        }).ToListAsync();

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

        public async Task<List<StaffDTO>> GetTop5ContributionStaffByYear(int year)
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
                    }).ToListAsync();

                    return await staffStatistic;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<StaffDTO>> GetTop5ContributionStaffByMonth(int month)
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
                   }).ToListAsync();

                    return await staffStatistic;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<decimal> GetTotalBenefitContributionOfStaffs(int year, int month = 0)
        {
            try
            {
                if (month == 0)
                {
                    using (var context = new CinemaManagementEntities())
                    {
                        decimal TotalBenefitContribution = await context.Bills.Where(b => b.CreatedAt.Year == year).SumAsync(b => (decimal?)b.TotalPrice) ?? 0;
                        return TotalBenefitContribution;
                    }
                }
                else
                {
                    using (var context = new CinemaManagementEntities())
                    {
                        decimal TotalBenefitContribution = await context.Bills.Where(b => b.CreatedAt.Year == year && b.CreatedAt.Month == month).SumAsync(b => (decimal?)b.TotalPrice) ?? 0;
                        return TotalBenefitContribution;
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #region Movie
        public async Task<List<MovieDTO>> GetTop5BestMovieByYear(int year)
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
                    }).ToListAsync();

                    return await movieStatistic;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<MovieDTO>> GetTop5BestMovieByMonth(int month)
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
                    }).ToListAsync();
                    return await movieStatistic;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Product
        public async Task<List<ProductDTO>> GetTop5BestProductByYear(int year)
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
                    }).ToListAsync();

                    return await prodStatistic;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<ProductDTO>> GetTop5BestProductByMonth(int month)
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
                    }).ToListAsync();
                    return await prodStatistic;
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
