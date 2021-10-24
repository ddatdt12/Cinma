using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.DTOs
{
    public class StaffDTO
    {
        public StaffDTO()
        {
            Role = ROLE.Staff;
        }
        public StaffDTO( int id, string name, string gender, DateTime birthday, string phonenumber,string role, DateTime startingdate)
        {
            Id = id; Name = name; Gender = gender; BirthDate = birthday; PhoneNumber = phonenumber;Role = role; StartingDate = startingdate;
        }



        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> StartingDate { get; set; }
        public string Role { get; set; }

        public virtual IList<BillDTO> Bills { get; set; }
    }
}
