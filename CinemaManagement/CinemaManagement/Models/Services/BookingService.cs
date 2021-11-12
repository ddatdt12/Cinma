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
        public (bool IsSuccess, string message) CreateTicketBooking(List<SeatSettingDTO> bookedSeatList)
        {
            if (bookedSeatList.Count() == 0)
            {
                return (false, "Vui lòng chọn ghế!");
            }
            var context = DataProvider.Ins.DB;
            try
            {
                var idSeatList = new List<int>();
                bookedSeatList.ForEach(s => idSeatList.Add(s.SeatId));
                var seatSets = context.SeatSettings.Where(s => s.ShowtimeId == bookedSeatList[0].ShowtimeId && idSeatList.Contains(s.SeatId));
                foreach (var s in seatSets)
                {
                    s.Status = true;
                }

                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Error Server {e}");
            }
            return (true, "Thêm phim thành công");
        }
    }
}
