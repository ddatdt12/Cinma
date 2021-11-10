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
            var context = DataProvider.Ins.DB;
            try
            {
                List<ProductDTO> productDTOs = (from p in context.Products
                                                where !p.IsDeleted
                                                select new ProductDTO
                                                {
                                                    Id = p.Id,
                                                    DisplayName = p.DisplayName,
                                                    Price = p.Price,
                                                    Category = p.Category,
                                                    Quantity = p.Quantity,
                                                    Image = p.Image
                                                }
                     ).ToList();
                return productDTOs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public (bool, string, ProductDTO) AddNewProduct(ProductDTO newProd)
        {
            try
            {
                var context = DataProvider.Ins.DB;

                Product prod = context.Products.Where((p) => p.DisplayName == newProd.DisplayName).FirstOrDefault();

                if (prod != null)
                {
                    if (prod.IsDeleted == false)
                    {
                        return (false, "Tên sản phầm đã tồn tại", null);
                    }

                    //Khi sản phẩm đã bị xóa nhưng được add lại với cùng tên 
                    prod.DisplayName = newProd.DisplayName;
                    prod.Price = newProd.Price;
                    prod.Category = newProd.Category;
                    prod.Image = newProd.Image;
                    prod.IsDeleted = false;
                    context.SaveChanges();
                    newProd.Id = prod.Id;
                }
                else
                {
                    Product product = new Product
                    {
                        DisplayName = newProd.DisplayName,
                        Price = newProd.Price,
                        Category = newProd.Category,
                        Image = newProd.Image,
                    };
                    context.Products.Add(product);
                    context.SaveChanges();
                    newProd.Id = product.Id;
                }

                return (true, "Thêm sản phẩm mới thành công", newProd);
            }
            catch (DbEntityValidationException e)
            {
                return (false, "DbEntityValidationException", null);
            }
            catch (Exception e)
            {
                    Console.WriteLine(e);
                return (false, $"Lỗi hệ thống {e}", null);
            }
        }

        public (bool, string) UpdateProduct(ProductDTO updatedProd)
        {
            var context = DataProvider.Ins.DB;
            try
            {
                Product prod = context.Products.Find(updatedProd.Id);

                if (prod is null)
                {
                    return (false, "Sản phẩm không tồn tại");
                }

                bool IsExistProdName = context.Products.Any((p) => p.Id != prod.Id && p.DisplayName == updatedProd.DisplayName);
                if (IsExistProdName)
                {
                    return (false, "Tên sản phẩm này đã tồn tại! Vui lòng chọn tên khác");
                }
                prod.DisplayName = updatedProd.DisplayName;
                prod.Price = updatedProd.Price;
                prod.Image = updatedProd.Image;
                prod.Category = updatedProd.Category;

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
        public (bool, string) DeleteProduct(int Id)
        {
            try
            {
                Product prod = (from p in DataProvider.Ins.DB.Products
                                where p.Id == Id && !p.IsDeleted
                                select p).FirstOrDefault();
                if (prod is null || prod?.IsDeleted == true)
                {
                    return (false, "Sản phẩm không tồn tại!");
                }
                prod.IsDeleted = true;

                DataProvider.Ins.DB.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Lỗi hệ thống {e.Message}");
            }
            return (true, "Xóa sản phẩm thành công");
        }
       

    }
}
