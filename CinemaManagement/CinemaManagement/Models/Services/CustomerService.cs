using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public CustomerDTO FindCustomerInfo(string phoneNumber)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var customer = context.Customers.Where(c => c.PhoneNumber == phoneNumber).FirstOrDefault();
                    if (customer is null)
                    {
                        return null;
                    }
                    return new CustomerDTO
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        PhoneNumber = customer.PhoneNumber
                    };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public (bool, string, string CustomerId) CreateNewCustomer(CustomerDTO newCus)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var isExistPhone = context.Customers.Any(c => c.PhoneNumber == newCus.PhoneNumber);
                    if (isExistPhone)
                    {
                        return (false, "Số điện thoại này đã tồn tại", null);
                    }
                    var isExistEmail = context.Customers.Any(c => c.Email == newCus.Email);
                    if (isExistPhone)
                    {
                        return (false, "Email này đã tồn tại", null);
                    }

                    Customer cus = new Customer
                    {
                        Name = newCus.Name,
                        PhoneNumber = newCus.PhoneNumber,
                        Email = newCus.Email,
                    };

                    context.Customers.Add(cus);
                    return (true, "", cus.Id);
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
    }
}
