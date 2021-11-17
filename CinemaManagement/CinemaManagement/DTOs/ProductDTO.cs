using CinemaManagement.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;

namespace CinemaManagement.DTOs
{
    public class ProductDTO
    {
        public ProductDTO()
        {
        }

        public ProductDTO(int id = 0,
                          string displayname = "Không xác định",
                          string category = "Không xác định",
                          decimal price = 0,
                          string imagesource = "",
                          int quantity = 1)
        {
            this.Id = id;
            this.DisplayName = displayname;
            this.Category = category;
            this.Price = price;
            this.Quantity = quantity;
            this.Image = imagesource;
        }
        public ProductDTO(ProductDTO product)
        {
            this.Id = product.Id;
            this.DisplayName = product.DisplayName;
            this.Category = product.Category;
            this.Price = product.Price;
            this.Quantity = product.Quantity;
            this.Image = product.Image;
            this.ImgSource = product.ImgSource;
        }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }

        public ImageSource _imgSource;
        public ImageSource ImgSource
        {
            get
            {
                if (_imgSource is null)
                {
                    if (File.Exists(Helper.GetMovieImgPath(Image)))
                    {
                        _imgSource = Helper.GetImageSource(Image);
                    }
                    else
                    {
                        _imgSource = Helper.GetImageSource("null.jpg");
                    }
                }
                return _imgSource;
            }
            set
            {
                _imgSource = value;
            }
        }
    }
}
