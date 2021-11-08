using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;
using CinemaManagement.Utils;
using System.Data.Entity;
using CinemaManagement.DTOs;
using System.Collections.Generic;
using System;
using System.Data.Entity.Validation;
using System.Linq;

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
                              ReleaseYear = movie.ReleaseYear,
                              MovieType = movie.MovieType,
                              Director = movie.Director,
                              Image = movie.Image,
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

        public List<MovieDTO> GetShowingMovieByDay(DateTime date)
        {
            List<MovieDTO> movieList = new List<MovieDTO>();
            var context = DataProvider.Ins.DB;
            try
            {
                var MovieIdList = (from showSet in context.ShowtimeSettings
                                   where DbFunctions.TruncateTime(showSet.ShowDate) == date.Date
                                   select showSet into S
                                   from show in S.Showtimes
                                   select new
                                   {
                                       MovieId = show.MovieId,
                                       ShowTime = show,
                                   }).GroupBy(m => m.MovieId).ToList();


                for (int i = 0; i < MovieIdList.Count(); i++)
                {
                    int id = MovieIdList[i].Key;

                    List<ShowtimeDTO> showtimeDTOsList = new List<ShowtimeDTO>();
                    MovieDTO mov = null;
                    foreach (var m in MovieIdList[i])
                    {
                        showtimeDTOsList.Add(new ShowtimeDTO
                        {
                            Id = m.ShowTime.Id,
                            MovieId = m.ShowTime.MovieId,
                            StartTime = m.ShowTime.StartTime,
                            RoomId = m.ShowTime.ShowtimeSetting.RoomId,
                            ShowDate = m.ShowTime.ShowtimeSetting.ShowDate,
                        });
                        if (mov is null)
                        {
                            Movie movie = m.ShowTime.Movie;
                            mov = new MovieDTO
                            {
                                Id = movie.Id,
                                DisplayName = movie.DisplayName,
                                RunningTime = movie.RunningTime,
                                Country = movie.Country,
                                Description = movie.Description,
                                ReleaseYear = movie.ReleaseYear,
                                MovieType = movie.MovieType,
                                Director = movie.Director,
                                Image = movie.Image,
                                Genres = (from genre in movie.Genres
                                          select new GenreDTO { DisplayName = genre.DisplayName, Id = genre.Id }
                                                 ).ToList(),
                            };
                        }
                    }
                    movieList.Add(mov);
                    movieList[i].Showtimes = showtimeDTOsList.OrderBy(s => s.StartTime).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return movieList;
        }

        public List<MovieDTO> GetShowingMovieByDay(DateTime date, int roomId)
        {
            List<MovieDTO> movieList = new List<MovieDTO>();
            var context = DataProvider.Ins.DB;
            try
            {
                var MovieIdList = (from showSet in context.ShowtimeSettings
                                   where DbFunctions.TruncateTime(showSet.ShowDate) == date.Date && showSet.RoomId == roomId
                                   select showSet into S
                                   from show in S.Showtimes
                                   select new
                                   {
                                       MovieId = show.MovieId,
                                       ShowTime = show,
                                   }).GroupBy(m => m.MovieId).ToList();


                for (int i = 0; i < MovieIdList.Count(); i++)
                {
                    int id = MovieIdList[i].Key;

                    List<ShowtimeDTO> showtimeDTOsList = new List<ShowtimeDTO>();
                    MovieDTO mov = null;
                    foreach (var m in MovieIdList[i])
                    {
                        showtimeDTOsList.Add(new ShowtimeDTO
                        {
                            Id = m.ShowTime.Id,
                            MovieId = m.ShowTime.MovieId,
                            StartTime = m.ShowTime.StartTime,
                            RoomId = m.ShowTime.ShowtimeSetting.RoomId,
                            ShowDate = m.ShowTime.ShowtimeSetting.ShowDate,
                        });
                        if (mov is null)
                        {
                            Movie movie = m.ShowTime.Movie;
                            mov = new MovieDTO
                            {
                                Id = movie.Id,
                                DisplayName = movie.DisplayName,
                                RunningTime = movie.RunningTime,
                                Country = movie.Country,
                                Description = movie.Description,
                                ReleaseYear = movie.ReleaseYear,
                                MovieType = movie.MovieType,
                                Director = movie.Director,
                                Image = movie.Image,
                                Genres = (from genre in movie.Genres
                                          select new GenreDTO { DisplayName = genre.DisplayName, Id = genre.Id }
                                                 ).ToList(),
                            };
                        }
                    }
                    movieList.Add(mov);
                    movieList[i].Showtimes = showtimeDTOsList.OrderBy(s => s.StartTime).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return movieList;
        }

        public (bool, string, MovieDTO) AddMovie(MovieDTO newMovie)
        {
            try
            {
                var context = DataProvider.Ins.DB;

                Movie m = context.Movies.Where((Movie mov) => mov.DisplayName == newMovie.DisplayName).FirstOrDefault();

                if (m != null)
                {
                    if (m.IsDeleted == false)
                    {
                        return (false, "Tên phim đã tồn tại", null);
                    }
                    //Khi phim đã bị xóa nhưng được add lại với cùng tên => update lại phim đã xóa đó với thông tin là 
                    // phim mới thêm thay vì add thêm
                    m.DisplayName = newMovie.DisplayName;
                    m.RunningTime = newMovie.RunningTime;
                    m.Country = newMovie.Country;
                    m.Description = newMovie.Description;
                    m.ReleaseYear = newMovie.ReleaseYear;
                    m.MovieType = newMovie?.MovieType;
                    m.Director = newMovie.Director;
                    m.Image = newMovie.Image;
                    foreach (var g in newMovie.Genres)
                    {
                        Genre genre = context.Genres.Find(g.Id);
                        m.Genres.Add(genre);
                    }
                    m.IsDeleted = false;
                    context.SaveChanges();
                    newMovie.Id = m.Id;
                }
                else
                {
                    Movie mov = new Movie
                    {
                        DisplayName = newMovie.DisplayName,
                        RunningTime = newMovie.RunningTime,
                        Country = newMovie.Country,
                        Description = newMovie.Description,
                        ReleaseYear = newMovie.ReleaseYear,
                        MovieType = newMovie?.MovieType,
                        Director = newMovie.Director,
                        Image = newMovie.Image,
                    };
                    foreach (var g in newMovie.Genres)
                    {
                        Genre genre = context.Genres.Find(g.Id);
                        mov.Genres.Add(genre);
                    }
                    context.Movies.Add(mov);
                    context.SaveChanges();
                    newMovie.Id = mov.Id;
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
                return (false, "DbEntityValidationException", null);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Error Server {e}", null);
            }
            return (true, "Thêm phim thành công", newMovie);
        }

        public (bool, string) UpdateMovie(MovieDTO updatedMovie)
        {
            var context = DataProvider.Ins.DB;
            try
            {
                Movie movie = context.Movies.Find(updatedMovie.Id);

                if (movie is null)
                {
                    return (false, "Phim không tồn tại");
                }

                bool IsExistMovieName = context.Movies.Any((Movie mov) => mov.Id != movie.Id && mov.DisplayName == updatedMovie.DisplayName);
                if (IsExistMovieName)
                {
                    return (false, "Tên phim đã tồn tại!");
                }

                PropertyCopier<MovieDTO, Movie>.Copy(updatedMovie, movie);
                context.SaveChanges();
                return (true, "Cập nhật thành công");
            }
            catch (DbEntityValidationException e)
            {
                return (false, "DbEntityValidationException");
            }
            catch (DbUpdateException e)
            {
                return (false, $"DbUpdateException: {e.Message}");
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }

        }
        public (bool, string) DeleteMovie(int Id)
        {
            try
            {
                Movie movie = (from p in DataProvider.Ins.DB.Movies
                               where p.Id == Id && !p.IsDeleted
                               select p).SingleOrDefault();
                if (movie == null || movie?.IsDeleted == true)
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
