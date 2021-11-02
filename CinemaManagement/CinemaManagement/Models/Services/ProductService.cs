using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public class ProductService
    {

        private ProductService() { }

        private static ProductService _ins;
        public static ProductService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ProductService();
                }
                return _ins;
            }
            private set => _ins = value;
        }

        public List<ProductDTO> GetAllProduct()
        {
            List<ProductDTO> movies = null;

            try
            {
                movies = (from movie in DataProvider.Ins.DB.Movies
                          where !movie.IsDeleted
                          select new ProductDTO
                          {
                              
                          }
                     ).ToList();

            }
            catch (Exception e)
            {
                throw e;
            }
            return movies;
        }



        public (bool, string, ProductDTO) AddProduct(ProductDTO newProd)
        {
            try
            {
                var context = DataProvider.Ins.DB;

                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                
                return (false, "DbEntityValidationException", null);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Error Server {e}", null);
            }
            return (true, "Thêm phim thành công", newProd);
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

                bool IsExistMovieName = context.Movies.Where((Movie mov) => mov.Id != movie.Id && mov.DisplayName == updatedMovie.DisplayName).Any();
                if (IsExistMovieName)
                {
                    return (false, "Tên phim đã tồn tại!");
                }

                context.SaveChanges();
                return (true, "Cập nhật thành công");
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
