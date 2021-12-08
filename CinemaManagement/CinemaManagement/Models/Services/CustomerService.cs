using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public class CustomerService
    {
        private static CustomerService _ins;
        public static CustomerService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new CustomerService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private CustomerService()
        {
        }

        public async Task<CustomerDTO> FindCustomerInfo(string phoneNumber)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var customer = await context.Customers.Where(c => c.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
                    if (customer is null)
                    {
                        return null;
                    }
                    return new CustomerDTO
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        PhoneNumber = customer.PhoneNumber,
                        Email = customer.Email,
                    };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private string CreateNextCustomerId(string maxId)
        {
            if (maxId is null)
            {
                return "KH0001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(2)) + 1}";
            return "KH" + newIdString.Substring(newIdString.Length - 4, 4);
        }
        public async Task<(bool, string, string CustomerId)> CreateNewCustomer(CustomerDTO newCus)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    bool isExistPhone = await context.Customers.AnyAsync(c => c.PhoneNumber == newCus.PhoneNumber);

                    if (isExistPhone)
                    {
                        return (false, "Số điện thoại này đã tồn tại", null);
                    }

                    bool isExistEmail = await context.Customers.AnyAsync(c => c.Email == newCus.Email);
                    if (isExistPhone)
                    {
                        return (false, "Email này đã tồn tại", null);
                    }
                    string currentMaxId = await context.Customers.MaxAsync(c => c.Id);
                    Customer cus = new Customer
                    {
                        Id = CreateNextCustomerId(currentMaxId),
                        Name = newCus.Name,
                        PhoneNumber = newCus.PhoneNumber,
                        Email = newCus.Email,
                        CreatedAt = DateTime.Now
                    };

                    context.Customers.Add(cus);
                    await context.SaveChangesAsync();
                    return (true, "", cus.Id);
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }

        public async Task<List<CustomerDTO>> GetTop5CustomerEmail()
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var cusStatistic = await context.Bills.Where(b => b.CreatedAt.Year == DateTime.Now.Year && b.CreatedAt.Month == DateTime.Now.Month)
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
                            Email = cus.Email
                        }).ToListAsync();
                    return cusStatistic;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<CustomerDTO>> GetNewCustomer()
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var customers = await context.Customers.Where(c => c.CreatedAt.Year == DateTime.Today.Year && DbFunctions.DiffDays(c.CreatedAt, DateTime.Now) <= 30)
                        .Select(c => new CustomerDTO
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Email = c.Email
                        }).ToListAsync();
                    return customers;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
