using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CinemaManagement.DTOs
{
    public class TroubleDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedAt { get; set; }
        public decimal RepairCost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Image
        {
            get; set;
        }
        public ImageSource _imgSource;
        public ImageSource ImgSource
        {
            get
            {
                if (_imgSource is null)
                {
                    if (File.Exists(Helper.GetMovieImgPath(Image)))
                    {
                        _imgSource = Helper.GetMovieImageSource(Image);
                    }
                    else
                    {
                        _imgSource = Helper.GetMovieImageSource("null.jpg");
                    }
                }
                return _imgSource;
            }
        }
        public string StaffId { get; set; }
        public string StaffName { get; set; }

    }
}
