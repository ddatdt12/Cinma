using CinemaManagement.Utils;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace CinemaManagement.DTOs
{
    public class ProductDTO
    {
        public ProductDTO()
        {
        }
        public ProductDTO(int id,
                          string displayname,
                          string category,
                          decimal price,
                          string image,
                          int quantity)
        {
            this.Id = id;
            this.DisplayName = displayname;
            this.Category = category;
            this.Price = price;
            this.Image = image;
            this.Quantity = quantity;
        }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string PriceStr
        {
            get
            {
                return Helper.FormatVNMoney(Price);
            }
        }
        public int Quantity { get; set; }
        public string Image
        {
            get; set;
        }
        public decimal Revenue { get; set; }
        public string RevenueStr
        {
            get
            {
                return Helper.FormatVNMoney(Revenue);
            }
        }
        public int SalesQuantity { get; set; }
    }
}