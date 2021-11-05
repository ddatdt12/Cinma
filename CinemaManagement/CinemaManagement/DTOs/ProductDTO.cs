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
                if (File.Exists(Helper.GetMovieImgPath(value)))
                {
                    ImgSource = Helper.GetImageSource(_image);
                }
                else
                {
                    _image = "null.jpg";
                    ImgSource = Helper.GetImageSource("null.jpg");
                }
            }
        }
        public ImageSource ImgSource { get; set; }

    }
}
