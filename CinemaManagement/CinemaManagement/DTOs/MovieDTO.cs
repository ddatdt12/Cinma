using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace CinemaManagement.DTOs
{
    public class MovieDTO
    {
        public MovieDTO()
        {
            MovieType = "2D";
            ReleaseYear = null;
        }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int RunningTime { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public Nullable<int> ReleaseYear { get; set; }
        public string MovieType { get; set; }
        public string Director { get; set; }
        public IList<ShowtimeDTO> Showtimes { get; set; }
        public IList<GenreDTO> Genres { get; set; }

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
                    if (File.Exists(Helper.GetMovieImgPath(_image)))
                    {
                        _imgSource = Helper.GetMovieImageSource(_image);
                    }
                    else
                    {
                        _imgSource = Helper.GetMovieImageSource("null.jpg");
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
