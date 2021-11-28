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

        public List<SeatSettingDTO> GetSeatsByShowtime(int showtimeId)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var seatList = (from s in context.SeatSettings
                                    where s.ShowtimeId == showtimeId
                                    select new SeatSettingDTO
                                    {
                                        SeatId = s.SeatId,
                                        ShowtimeId = s.ShowtimeId,
                                        Status = s.Status,
                                        Seat = new SeatDTO
                                        {
                                            Id = s.Seat.Id,
                                            RoomId = s.Seat.RoomId,
                                            Row = s.Seat.Row,
                                            SeatNumber = s.Seat.SeatNumber,
                                        },
                                    }
                               ).ToList();
                    return seatList;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void SettingSeatForNewShowtime(CinemaManagementEntities context, int roomId, int showtimeId)
        {
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
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
