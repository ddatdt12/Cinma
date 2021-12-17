using CinemaManagement.Utils;
using System;
using System.IO;
using System.Windows.Media;

namespace CinemaManagement.DTOs
{
    public class TroubleDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string RepairCostStr
        {
            get
            {
                return Helper.FormatVNMoney(RepairCost);
            }
        }
        public decimal RepairCost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public byte[] Image
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
                    if (Image is null)
                    {
                        _imgSource = Helper.GetNullImageSource("null.jpg");
                    }
                    else
                    {
                        _imgSource = Helper.ConvertByteToImageSource(Image);
                    }
                }
                return _imgSource;
            }
        }
        public string StaffId { get; set; }
        public string StaffName { get; set; }

    }
}
