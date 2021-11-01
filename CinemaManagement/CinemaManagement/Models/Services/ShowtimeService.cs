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

       
        public (bool IsSuccess, string message, ShowtimeDTO) AddShowtime(ShowtimeDTO newShowtime)
        {

            var context = DataProvider.Ins.DB;
            try
            {

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
                    var newEndTime = newShowtime.StartTime + new TimeSpan(0,m.RunningTime,0);
                    show = showtimeSet.Showtimes.AsEnumerable().Where(s =>
                    {
                        var endTime = new TimeSpan(0, s.Movie.RunningTime, 0) + s.StartTime;
                        return TimeBetwwenIn(newStartTime, newEndTime, s.StartTime, endTime + TIME.BreakTime);
                    }).FirstOrDefault();

                    if (show != null)
                    {
                        var endTime = new TimeSpan(0, show.Movie.RunningTime, 0) + show.StartTime;
                        return (false, $"Khoảng thời gian từ {Helper.GetHourMinutes(show.StartTime)} đến {Helper.GetHourMinutes(endTime + TIME.BreakTime)} đã có phim chiếu tại phòng {showtimeSet.RoomId}", null);
                    }
                }

               

                Showtime showtime = new Showtime
                {
                    MovieId = newShowtime.MovieId,
                    ShowtimeSettingId = showtimeSet.Id,
                    StartTime = newShowtime.StartTime,
                };

                //Lack of setting seats in room for new showtime 

                context.Showtimes.Add(showtime);
                context.SaveChanges();

                newShowtime.Id = showtime.Id;
                return (true, "Thêm xuất chiếu thành công" , newShowtime);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return (false, "Lỗi DbEntityValidationException", null);

            }
            catch (DbUpdateException e)
            {
                return (false, "Lỗi DbUpdateException", null);

            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);

            }
        }
        public (bool IsSuccess, string message) DeleteShowtime(int showtimeId) {

            var context = DataProvider.Ins.DB;
            try
            {
                Showtime show = context.Showtimes.Find(showtimeId);
                context.Showtimes.Remove(show);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");   
            }
            return (true, "Xóa suất chiếu thành công!");
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
