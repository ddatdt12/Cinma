using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;
using CinemaManagement.Utils;

namespace CinemaManagement.Models.Services
{
    public class MovieService
    {

        private MovieService() { }

        private static MovieService _ins;
        public static MovieService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new MovieService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        
        public List<MovieDTO> GetAllMovie()
        {
            List<MovieDTO> movies = null;

            try
            {
                movies = (from movie in DataProvider.Ins.DB.Movies
                          where !movie.IsDeleted
                          select new MovieDTO
                          {
                              Id = movie.Id,
                              DisplayName = movie.DisplayName,
                              RunningTime = movie.RunningTime,
                              Country = movie.Country,
                              Description = movie.Description,
                              ReleaseDate = movie.ReleaseDate,
                              MovieType = movie.MovieType,
                              Director = movie.Director,
                              Genres = (from genre in movie.Genres
                                        select new GenreDTO { DisplayName = genre.DisplayName, Id = genre.Id }
                                      ).ToList(),
                          }
                     ).ToList();

            }
            catch (Exception e)
            {
                throw e;
            }
            return movies;
        }

        public (bool, string) AddMovie(MovieDTO newMovie)
        {
            try
            {
                var context = DataProvider.Ins.DB;

                Movie m = context.Movies.Where((Movie mov) => mov.DisplayName == newMovie.DisplayName).FirstOrDefault();


                if (m != null)
                {
                    if (m.IsDeleted == false)
                    {
                        return (false, "Tên phim đã tồn tại");
                    }
                    //Khi phim đã bị xóa nhưng được add lại với cùng tên => update lại phim đã xóa đó với thông tin là 
                    // phim mới thêm thay vì add thêm
                    PropertyCopier<MovieDTO, Movie>.Copy(newMovie, m);
                    foreach (var g in newMovie.Genres)
                    {
                        Genre genre = context.Genres.Find(g.Id);
                        m.Genres.Add(genre);
                    }
                    m.IsDeleted = false;
                }
                else
                {
                    Movie mov = new Movie();
                    PropertyCopier<MovieDTO, Movie>.Copy(newMovie, mov);
                    foreach (var g in newMovie.Genres)
                    {
                        Genre genre = context.Genres.Find(g.Id);
                        mov.Genres.Add(genre);
                    }
                    context.Movies.Add(mov);

                    //context.Movies.Add(new Movie
                    //{
                    //    DisplayName = movie.DisplayName,
                    //    RunningTime = movie.RunningTime,
                    //    Country = movie.Country,
                    //    Description = movie.Description,
                    //    ReleaseDate = movie.ReleaseDate,
                    //    MovieType = movie?.MovieType,
                    //    Director = movie.Director
                    //});
                }
                

                context.SaveChanges();
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
                return (false, "DbEntityValidationException");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, "Error Server");
            }
            return (true, "Thêm phim thành công");
        }

        public (bool, string) UpdateMovie(MovieDTO updatedMovie)
        {
            try
            {
                Movie movie = DataProvider.Ins.DB.Movies.Find(updatedMovie.Id);

                if (movie == null)
                {
                    return (false, "Phim không tồn tại");
                }

                if (movie.DisplayName == updatedMovie.DisplayName)
                {
                    return (false, "Tên phim đã tồn tại!");
                }

                PropertyCopier<MovieDTO, Movie>.Copy(updatedMovie, movie);

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
                return (false, "DbEntityValidationException");

            }
            catch (DbUpdateException e)
            {
                return (false, $"DbUpdateException: {e.Message}");
            }
            catch (Exception)
            {
                return (false, "Error Server");
            }
            return (true, "Cập nhật thành công");

        }
        public (bool, string) DeleteMovie(int Id)
        {
            try
            {
                Movie movie = (from p in DataProvider.Ins.DB.Movies
                                 where p.Id == Id && !p.IsDeleted
                               select p).SingleOrDefault();
                if(movie == null || movie?.IsDeleted == true)
                {
                    return (false, "Phim không tồn tại!");
                }
                movie.IsDeleted = true;

                DataProvider.Ins.DB.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Error Server {e.Message}");
            }
            return (true, "Xóa phim thành công");
        }
    }
}
