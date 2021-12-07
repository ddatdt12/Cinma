using CinemaManagement.Utils;
using System;
using System.Collections.Generic;

namespace CinemaManagement.DTOs
{
    public class StaffDTO
    {
        public StaffDTO()
        {
            Role = ROLE.Staff;
        }

        public StaffDTO(string id, string name, string gender, DateTime birthday, string phonenumber, string role, DateTime startingdate)
        {
            Id = id; Name = name; Gender = gender; BirthDate = birthday; PhoneNumber = phonenumber; Role = role; StartingDate = startingdate;
        }

        private int GetAge(DateTime birthDate)
        {
            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - birthDate.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (birthDate.DayOfYear > today.DayOfYear) age--;

            return age;
        }


        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<DateTime> BirthDate
        {
            get; set;
        }
        public string Gender { get; set; }
        public Nullable<System.DateTime> StartingDate { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public virtual IList<BillDTO> Bills { get; set; }


        //Statistic
        public decimal BenefitContribution { get; set; }
        public string BenefitContributionStr
        {
            get
            {
                return Helper.FormatVNMoney(BenefitContribution);
            }
        }
    }
}
