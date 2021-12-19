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

        public async Task<List<ProductDTO>> GetAllProduct()
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    List<ProductDTO> productDTOs = await (from p in context.Products
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
                     ).ToListAsync();
                    return productDTOs;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public async Task<(bool, string, ProductDTO)> AddNewProduct(ProductDTO newProd)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {

                    Product prod = await context.Products.Where((p) => p.DisplayName == newProd.DisplayName).FirstOrDefaultAsync();

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
                        await context.SaveChangesAsync();
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
                        await context.SaveChangesAsync();
                        newProd.Id = product.Id;
                    }

                    return (true, "Thêm sản phẩm mới thành công", newProd);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Lỗi hệ thống {e}", null);
            }
        }

        public async Task<(bool, string)> UpdateProduct(ProductDTO updatedProd)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    Product prod = await context.Products.FindAsync(updatedProd.Id);

                    if (prod is null)
                    {
                        return (false, "Sản phẩm không tồn tại");
                    }

                    bool IsExistProdName = await context.Products.AnyAsync((p) => p.Id != prod.Id && p.DisplayName == updatedProd.DisplayName);
                    if (IsExistProdName)
                    {
                        return (false, "Tên sản phẩm này đã tồn tại! Vui lòng chọn tên khác");
                    }
                    prod.DisplayName = updatedProd.DisplayName;
                    prod.Price = updatedProd.Price;
                    prod.Image = updatedProd.Image;
                    prod.Category = updatedProd.Category;

                    await context.SaveChangesAsync();
                    return (true, "Cập nhật thành công");
                }
            }
            catch (DbEntityValidationException e)
            {
                return (false, $"DbEntityValidationException {e.Message}");

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
        public async Task<(bool, string)> DeleteProduct(int Id)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    Product prod = await (from p in context.Products
                                    where p.Id == Id && !p.IsDeleted
                                    select p).FirstOrDefaultAsync();
                    if (prod is null)
                    {
                        return (false, "Sản phẩm không tồn tại!");
                    }

                    if (prod.Image != null)
                    {
                        CloudinaryService.Ins.DeleteImage(prod.Image);
                        prod.Image = null;
                    }
                    prod.IsDeleted = true;

                    await context.SaveChangesAsync();
                }
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