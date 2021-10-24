using CinemaManagement.DTOs;
using System;
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


        //public Nullable<int> ShowtimeSettingId { get; set; }
        //public Nullable<int> MovieId { get; set; }
        //public Nullable<System.TimeSpan> StartTime { get; set; }

        //public MovieDTO Movie { get; set; }
        //public virtual IList<TicketDTO> Tickets { get; set; }
        //public List<ShowtimeDTO> GetAllGenre()
        //{
        //    //List<ShowtimeDTO> genres;
        //    //try
        //    //{
        //    //    var context = DataProvider.Ins.DB;
        //    //    genres = (from s in context.Showtimes
        //    //              select new ShowtimeDTO { Id = s.Id, DisplayName = s.DisplayName }).ToList();
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    throw e;
        //    //}

        //    return genres;
        //}

        public (bool, string message) AddGenre(GenreDTO genre)
        {
            try
            {
                var genreInDB = DataProvider.Ins.DB.Genres.Where(g => g.DisplayName == genre.DisplayName).FirstOrDefault();
                if (genreInDB != null)
                {
                    return (false, "Thể loại phim này đã tồn tại");
                }
                DataProvider.Ins.DB.Genres.Add(new Genre
                {
                    DisplayName = genre.DisplayName,
                });
                DataProvider.Ins.DB.SaveChanges();

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
            return (true, "");
        }

    }
}
