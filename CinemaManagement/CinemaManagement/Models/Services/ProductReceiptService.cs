using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public class ProductReceiptService
    {
        private ProductReceiptService() { }

        private static ProductReceiptService _ins;
        public static ProductReceiptService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ProductReceiptService();
                }
                return _ins;
            }
            private set => _ins = value;
        }

        public async Task<List<ProductReceiptDTO>> GetProductReceipt()
        {
            List<ProductReceiptDTO> productReceipts;
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    productReceipts = await (from pr in context.ProductReceipts
                                             orderby pr.CreatedAt descending
                                             select new ProductReceiptDTO
                                             {
                                                 Id = pr.Id,
                                                 ProductId = pr.ProductId,
                                                 ProductName = pr.Product.DisplayName,
                                                 StaffId = pr.Staff.Id,
                                                 StaffName = pr.Staff.Name,
                                                 Quantity = pr.Quantity,
                                                 ImportPrice = pr.ImportPrice,
                                                 CreatedAt = pr.CreatedAt,
                                             }).ToListAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return productReceipts;
        }

        public async Task<List<ProductReceiptDTO>> GetProductReceipt(int month)
        {
            List<ProductReceiptDTO> productReceipts;
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    productReceipts = await (from pr in context.ProductReceipts
                                             where pr.CreatedAt.Year == DateTime.Today.Year && pr.CreatedAt.Month == month
                                             orderby pr.CreatedAt descending
                                             select new ProductReceiptDTO
                                             {
                                                 Id = pr.Id,
                                                 ProductId = pr.ProductId,
                                                 ProductName = pr.Product.DisplayName,
                                                 StaffId = pr.Staff.Id,
                                                 StaffName = pr.Staff.Name,
                                                 Quantity = pr.Quantity,
                                                 ImportPrice = pr.ImportPrice,
                                                 CreatedAt = pr.CreatedAt,
                                             }).ToListAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return productReceipts;
        }
        private string CreateNextProdReceiptId(string maxId)
        {
            //NVxxx
            if (maxId is null)
            {
                return "PRC001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(3)) + 1}";
            return "PRC" + newIdString.Substring(newIdString.Length - 3, 3);
        }
        public async Task<(bool, string, ProductReceiptDTO)> CreateProductReceipt(ProductReceiptDTO newPReceipt)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    Product prod = await context.Products.FindAsync(newPReceipt.ProductId);
                    prod.Quantity += newPReceipt.Quantity;

                    string maxId = context.ProductReceipts.Max(pr => pr.Id);

                    ProductReceipt pR = new ProductReceipt
                    {
                        Id = CreateNextProdReceiptId(maxId),
                        ImportPrice = newPReceipt.ImportPrice,
                        ProductId = newPReceipt.ProductId,
                        CreatedAt = DateTime.Now,
                        Quantity = newPReceipt.Quantity,
                        StaffId = newPReceipt.StaffId,
                    };
                    context.ProductReceipts.Add(pR);
                    await context.SaveChangesAsync();

                    newPReceipt.Id = pR.Id;
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
            return (true, "Lưu thông tin nhập hàng thành công", newPReceipt);
        }
    }
}
