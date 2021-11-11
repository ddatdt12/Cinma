using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public class BookingService
    {
        private static BookingService _ins;
        public static BookingService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new BookingService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private BookingService()
        {
        }

        /// <summary>
        ///  Đặt những vé xem phim khi biết danh sách ghế
        /// </summary>
        /// <param name="bookedSeatList"></param>
        /// <returns></returns>
        //public (bool IsSuccess, string message, ShowtimeDTO newShowtime) CreateTicketBooking(List<SeatSettingDTO> bookedSeatList)
        //{
        //        var context = DataProvider.Ins.DB;
        //    try
        //    {
        //            context.Movies.Add(mov);
        //            context.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {

        //        return (false, "DbEntityValidationException", null);

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        return (false, $"Error Server {e}", null);
        //    }
        //    return (true, "Thêm phim thành công", newMovie);
        //}
    }
}
