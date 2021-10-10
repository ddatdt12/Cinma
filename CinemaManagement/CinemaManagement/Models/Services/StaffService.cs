using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public class StaffService
    {

        public static List<Staff> GetAllUser()
        {
            List<Staff> staffs = null;

            using (var context = new CinemaManagementEntities())
            {

                staffs = context.Staffs.ToList();
            }

            return staffs;
        }

     
    }
}
