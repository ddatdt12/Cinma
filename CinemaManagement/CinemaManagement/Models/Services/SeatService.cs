using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public class SeatService
    {
        public (bool IsSuccess, string message) SettingSeatForNewShowtime(string showtimeId)
        {
            var context = DataProvider.Ins.DB;

            try
            {
             
            }
            catch (Exception e)
            {
                throw e;
            }

            return (true, "");
        }

    }
}
