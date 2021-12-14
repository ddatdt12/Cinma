using CinemaManagement.Utils;
using System;

namespace CinemaManagement.DTOs
{
    public class CustomerDTO
    {
        public CustomerDTO()
        {
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }

        //Expense
        public decimal Expense { get; set; }
        public string ExpenseStr
        {
            get
            {
                return Helper.FormatVNMoney(Expense);
            }
        }

        //public virtual IList<BillDTO> Bills { get; set; }
    }
}
