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
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        private string _image;
        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                if (File.Exists(Helper.GetProductImgPath(_image)))
                {
                    ImgSource = Helper.GetProductImageSource(_image);
                }
                else
                {
                    _image = "null.jpg";
                    ImgSource = Helper.GetProductImageSource("null.jpg");
                }
            }
        }
        public ImageSource ImgSource { get; set; }

    }
}
