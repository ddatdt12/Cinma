using CinemaManagement.DTOs;
using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var context = DataProvider.Ins.DB;
            try
            {
                var showtimeSet = context.ShowtimeSettings
                    .Where(s => DbFunctions.TruncateTime(s.ShowDate) == newShowtime.ShowDate.Date
                    && s.RoomId == newShowtime.RoomId).FirstOrDefault();

                Showtime show = null;
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
                    show = showtimeSet.Showtimes.AsEnumerable().Where(s =>
                    {
                        var endTime = new TimeSpan(0, s.Movie.RunningTime, 0) + s.StartTime;
                        var newStartTime = newShowtime.StartTime;
                        return newStartTime >= s.StartTime && newStartTime < (endTime + TIME.BreakTime);
                    }).FirstOrDefault();
                }

                if (show != null)
                {
                    var endTime = new TimeSpan(0, show.Movie.RunningTime, 0) + show.StartTime;
                    return (false, $"Khoảng thời gian từ {Helper.GetHourMinutes(show.StartTime)} đến {Helper.GetHourMinutes(endTime + TIME.BreakTime)} đã có phim chiếu tại phòng {showtimeSet.RoomId}");
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

                return (true, "Thêm xuất chiếu thành công");
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
                return (false, e.Message);

            }
            catch (DbUpdateException e)
            {
                return (false, e?.InnerException.Message);
            }
            catch (Exception e)
            {
                return (false, e?.InnerException.Message);
            }
        }

    }
}
