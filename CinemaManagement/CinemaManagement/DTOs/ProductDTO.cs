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
            }
        }

        public ImageSource _imgSource;
        public ImageSource ImgSource
        {
            get
            {
                if (_imgSource is null)
                {
                    if (File.Exists(Helper.GetProductImgPath(_image)))
                    {
                        _imgSource = Helper.GetProductImageSource(_image);
                    }
                    else
                    {
                        _imgSource = Helper.GetProductImageSource("null.jpg");
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
