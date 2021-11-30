using CinemaManagement.DTOs;
using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace CinemaManagement.Models.Services
{
    public class ShowtimeService
    {
        private static ShowtimeService _ins;
        public static ShowtimeService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ShowtimeService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private ShowtimeService() { }


        public (bool IsSuccess, string message) AddShowtime(ShowtimeDTO newShowtime)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    //Uncomment when release
                    //if (newShowtime.ShowDate < DateTime.Today)
                    //{
                    //    return (false,"Thời gian này đã qua không thể thêm suất chiếu" ,null);
                    //}
                    var showtimeSet = context.ShowtimeSettings
                    .Where(s => DbFunctions.TruncateTime(s.ShowDate) == newShowtime.ShowDate.Date
                    && s.RoomId == newShowtime.RoomId).FirstOrDefault();

                    if (showtimeSet == null)
                    {
                        showtimeSet = new ShowtimeSetting
                        {
                            RoomId = newShowtime.RoomId,
                            ShowDate = newShowtime.ShowDate.Date,
                        }; ;
                        context.ShowtimeSettings.Add(showtimeSet);
                        context.SaveChanges();
                    }
                    else
                    {
                        Showtime show = null;

                        Movie m = context.Movies.Find(newShowtime.MovieId);
                        var newStartTime = newShowtime.StartTime;
                        var newEndTime = newShowtime.StartTime + new TimeSpan(0, m.RunningTime, 0);
                        show = showtimeSet.Showtimes.AsEnumerable().Where(s =>
                        {
                            var endTime = new TimeSpan(0, s.Movie.RunningTime, 0) + s.StartTime;
                            return TimeBetwwenIn(newStartTime, newEndTime, s.StartTime, endTime + TIME.BreakTime);
                        }).FirstOrDefault();

                        if (show != null)
                        {
                            var endTime = new TimeSpan(0, show.Movie.RunningTime, 0) + show.StartTime;
                            return (false, $"Khoảng thời gian từ {Helper.GetHourMinutes(show.StartTime)} đến {Helper.GetHourMinutes(endTime + TIME.BreakTime)} đã có phim chiếu tại phòng {showtimeSet.RoomId}");
                        }
                    }

                    Showtime showtime = new Showtime
                    {
                        MovieId = newShowtime.MovieId,
                        ShowtimeSettingId = showtimeSet.Id,
                        StartTime = newShowtime.StartTime,
                        TicketPrice = newShowtime.TicketPrice
                    };
                    context.Showtimes.Add(showtime);

                    //setting seats in room for new showtime 
                    SeatService.Ins.SettingSeatForNewShowtime(context, showtimeSet.RoomId, showtime.Id);

                    context.SaveChanges();
                    newShowtime.Id = showtime.Id;
                    return (true, "Thêm suất chiếu thành công");
                }
            }
            catch (DbEntityValidationException e)
            {
                return (false, "Lỗi DbEntityValidationException" + e.Message);

            }
            catch (DbUpdateException e)
            {
                return (false, "Lỗi DbUpdateException" + e.Message);

            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống" + e.Message);

            }
        }
        public (bool IsSuccess, string message) DeleteShowtime(int showtimeId)
        {

            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    Showtime show = context.Showtimes.Find(showtimeId);
                    if (show is null)
                    {
                        return (false, "Suất chiếu không tồn tại!");
                    }
                    context.Showtimes.Remove(show);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
            return (true, "Xóa suất chiếu thành công!");
        }
        public (bool IsSuccess, string message) UpdateTicketPrice(int showtimeId, decimal price)
        {

            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    Showtime show = context.Showtimes.Find(showtimeId);
                    if (show is null)
                    {
                        return (false, "Suất chiếu không tồn tại!");
                    }
                    show.TicketPrice = price;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
            return (true, "Cập nhật giá thành công!");
        }
        public bool CheckShowtimeHaveBooking(int showtimeId)
        {

            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var IsExist = context.SeatSettings.Any(s => s.ShowtimeId == showtimeId && s.Status);
                    return IsExist;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Check (t1,t2) vs (a1,a2)
        bool TimeBetwwenIn(TimeSpan t1, TimeSpan t2, TimeSpan a1, TimeSpan a2)
        {

            if ((t1 >= a1 && t1 <= a2) || (t2 >= a1 && t2 <= a2))
                return true;
            if (t1 <= a1 && t2 >= a2)
            {
                return true;
            }
            // t2 > t1;
            return false;
        }

    }
}
