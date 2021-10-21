using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;

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
            List<MovieDTO> movies  = null;

            try
            {
                movies = (from movie in DataProvider.Ins.DB.Movies
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
      
        public (bool, string) AddMovie(MovieDTO movie)
        {
            try
            {
                DataProvider.Ins.DB.Movies.Add(new Movie
                {
                    DisplayName = movie.DisplayName,
                    RunningTime = movie.RunningTime,
                    Country = movie.Country,
                    Description = movie.Description,
                    ReleaseDate = movie.ReleaseDate,
                    MovieType = movie?.MovieType,
                    Director = movie.Director
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
                return (false, "DbEntityValidationException");

            }
            catch (DbUpdateException)
            {
                return (false, $"DbUpdateException: {e.Message}");
            }
            catch (Exception)
            {
                return (false, "Error Server");
            }
            return (true, "");
        }

        public (bool, string) UpdateMovie(MovieDTO movie)
        {
            try
            {
                DataProvider.Ins.DB.Movies.Add(new Movie
                {
                    DisplayName = movie.DisplayName,
                    RunningTime = movie.RunningTime,
                    Country = movie.Country,
                    Description = movie.Description,
                    ReleaseDate = movie.ReleaseDate,
                    MovieType = movie?.MovieType,
                    Director = movie.Director
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
            return (true, "");
        }
    }
}
