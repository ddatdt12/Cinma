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
                    var customer = await context.Customers.Where(c => !c.IsDeleted && c.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
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

        public async Task<List<CustomerDTO>> GetAllCustomerByTime(int year, int month = 0)
        {
            try
            {
                if (month == 0)
                {
                    using (var context = new CinemaManagementEntities())
                    {
                        var customer = await context.Customers.Where(c => !c.IsDeleted && c.CreatedAt.Year == year).Select(c => new CustomerDTO
                        {
                            Id = c.Id,
                            Name = c.Name,
                            PhoneNumber = c.PhoneNumber,
                            Email = c.Email,
                            StartDate = c.CreatedAt,
                            Expense = c.Bills.Where(b => b.CreatedAt.Year == year).Sum(b => (decimal?)b.TotalPrice) ?? 0
                        }).ToListAsync();

                        return customer;
                    }
                }
                else
                {
                    using (var context = new CinemaManagementEntities())
                    {
                        var customer = await context.Customers.Where(c => !c.IsDeleted && c.CreatedAt.Year == DateTime.Today.Year && c.CreatedAt.Month == month)
                            .Select(c => new CustomerDTO
                        {
                            Id = c.Id,
                            Name = c.Name,
                            PhoneNumber = c.PhoneNumber,
                            Email = c.Email,
                            StartDate = c.CreatedAt,
                            Expense = c.Bills.Where(b => b.CreatedAt.Year == year && b.CreatedAt.Month == month).Sum(b => (decimal?)b.TotalPrice) ?? 0
                        }).ToListAsync();

                        return customer;
                    }
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
                    if (newCus.Email != null)
                    {
                        bool isExistEmail = await context.Customers.AnyAsync(c => c.Email == newCus.Email);
                        if (isExistEmail)
                        {
                            return (false, "Email này đã tồn tại", null);
                        }
                    }

                    var cus = await context.Customers.Where(c => c.PhoneNumber == newCus.PhoneNumber).FirstOrDefaultAsync();
                    if (cus != null)
                    {
                        if (!cus.IsDeleted)
                        {
                            return (false, "Số điện thoại này đã tồn tại", null);
                        }
                        else
                        {
                            cus.Name = newCus.Name;
                            cus.Email = newCus.Email;
                            cus.CreatedAt = DateTime.Now;
                            cus.IsDeleted = false;
                        }

                        await context.SaveChangesAsync();
                        return (true, "Đăng ký thành công", cus.Id);
                    }


                    string currentMaxId = await context.Customers.MaxAsync(c => c.Id);
                    Customer newCusomer = new Customer
                    {
                        Id = CreateNextCustomerId(currentMaxId),
                        Name = newCus.Name,
                        PhoneNumber = newCus.PhoneNumber,
                        Email = newCus.Email,
                        CreatedAt = DateTime.Now,
                    };

                    context.Customers.Add(newCusomer);
                    await context.SaveChangesAsync();
                    return (true, "Đăng ký thành công", newCusomer.Id);
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }

        public async Task<(bool, string)> UpdateCustomerInfo(CustomerDTO updatedCus)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    bool isExistPhone = await context.Customers.AnyAsync(c => c.Id != updatedCus.Id && c.PhoneNumber == updatedCus.PhoneNumber);

                    if (isExistPhone)
                    {
                        return (false, "Số điện thoại này đã tồn tại");
                    }

                    if (!string.IsNullOrEmpty(updatedCus.Email))
                    {
                        bool isExistEmail = await context.Customers.AnyAsync(c => c.Id != updatedCus.Id && c.Email == updatedCus.Email);
                        if (isExistEmail)
                        {
                            return (false, "Email này đã tồn tại");
                        }
                    }
                    var cus = await context.Customers.FindAsync(updatedCus.Id);

                    cus.Name = updatedCus.Name;
                    cus.PhoneNumber = updatedCus.PhoneNumber;
                    cus.Email = updatedCus.Email;

                    await context.SaveChangesAsync();
                    return (true, "Cập nhật thành công");
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
        }

        public async Task<(bool, string)> DeleteCustomer(string id)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var cus = await context.Customers.FindAsync(id);
                    if (cus is null || cus.IsDeleted)
                    {
                        return (false, "Khách hàng không tồn tại!");
                    }

                    cus.IsDeleted = true;
                    await context.SaveChangesAsync();
                    return (true, "Xóa thành công");
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
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
