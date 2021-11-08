using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public class SeatService
    {

        private static SeatService _ins;
        public static SeatService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new SeatService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private SeatService() { }

        public (bool isSuccess, string message, List<SeatSettingDTO>) GetSeatsByShowtime(int showtimeId)
        {

            var context = DataProvider.Ins.DB;
            try
            {
                var seatList = (from s in context.SeatSettings
                                where s.ShowtimeId == showtimeId
                                select new SeatSettingDTO
                                {
                                    SeatId=s.SeatId,
                                    ShowtimeId=s.ShowtimeId,
                                    Status=s.Status
                                }
                           ).DefaultIfEmpty().ToList();
                return (true, null, seatList);
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
        public (bool IsSuccess, string message) SettingSeatForNewShowtime(int roomId, int showtimeId)
        {

            var context = DataProvider.Ins.DB;

            try
            {
                var seatIds = (from s in context.Seats
                               where s.RoomId == roomId
                               select s.Id
                           ).ToList();
                List<SeatSetting> seatSetList = new List<SeatSetting>();
                foreach (var seatId in seatIds)
                {
                    seatSetList.Add(new SeatSetting
                    {
                        SeatId = seatId,
                        ShowtimeId = showtimeId
                    });
                }
                context.SeatSettings.AddRange(seatSetList);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
            return (true, "");
        }

    }
}
