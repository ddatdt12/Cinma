using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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

        public List<ProductReceiptDTO> GetProductReceipt()
        {
            List<ProductReceiptDTO> productReceipts;
            try
            {
                productReceipts = (from pr in DataProvider.Ins.DB.ProductReceipts
                                   select new ProductReceiptDTO
                                   {
                                       Id = pr.Id,
                                       ProductId =pr.ProductId,
                                       ProductName = pr.Product.DisplayName,
                                       StaffId = pr.Staff.Id,
                                       StaffName = pr.Staff.Name,
                                       Quantity = pr.Quantity,
                                       ImportPrice = pr.ImportPrice,
                                       CreatedAt = pr.CreatedAt,
                                   }).ToList();

                DataProvider.Ins.DB.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return productReceipts;
        }

        public List<ProductReceiptDTO> GetProductReceipt(DateTime date)
        {
            List<ProductReceiptDTO> productReceipts;
            try
            {
                productReceipts = (from pr in DataProvider.Ins.DB.ProductReceipts
                                   where DbFunctions.TruncateTime(pr.CreatedAt) == date.Date
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
                                   }).ToList();

                DataProvider.Ins.DB.SaveChanges();
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
        public (bool ,string, ProductReceiptDTO) CreateProductReceipt(ProductReceiptDTO newPReceipt)
        {

            var context = DataProvider.Ins.DB;
            try
            {
                Product prod = context.Products.Find(newPReceipt.ProductId);
                prod.Quantity += newPReceipt.Quantity;

                string maxId = context.ProductReceipts.Max(pr => pr.Id);

                ProductReceipt pR = new ProductReceipt
                {
                    Id = CreateNextProdReceiptId(maxId),
                    ImportPrice = newPReceipt.ImportPrice,
                    ProductId = newPReceipt.ProductId,
                    CreatedAt = DateTime.Today,
                    Quantity = newPReceipt.Quantity,
                    StaffId = newPReceipt.StaffId,
                };
                context.ProductReceipts.Add(pR);

                context.SaveChanges();

                newPReceipt.Id = pR.Id;
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
            return (true, "Lưu thông tin nhập hàng thành công", newPReceipt);
        }
    }
}
