using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace CinemaManagement.Models.Services
{
    public class GenreService
    {
        private GenreService() { }
        private static GenreService _ins;
        public static GenreService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new GenreService();
                }
                return _ins;
            }
            private set => _ins = value;
        }

        public List<GenreDTO> GetAllGenre()
        {
            List<GenreDTO> genres;
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    genres = (from s in context.Genres
                              select new GenreDTO { Id = s.Id, DisplayName = s.DisplayName }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return genres;
        }

        public (bool, string message) AddGenre(GenreDTO genre)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var genreInDB = context.Genres.Where(g => g.DisplayName == genre.DisplayName).FirstOrDefault();
                    if (genreInDB != null)
                    {
                        return (false, "Thể loại phim này đã tồn tại");
                    }
                    context.Genres.Add(new Genre
                    {
                        DisplayName = genre.DisplayName,
                    });
                    context.SaveChanges();
                }

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

        public (bool, string message) EditGenre(int GenreId, string newDisplayName)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var genre = context.Genres.Where(g => g.Id == GenreId).FirstOrDefault();
                    if (genre == null)
                    {
                        return (false, "Genre don't exist");
                    }
                    genre.DisplayName = newDisplayName;
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                return (false, e.Message);

            }
            catch (DbUpdateException e)
            {
                return (false, e.Message);
            }
            return (true, "");

        }


    }
}
