using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.DTOs
{
    public class TicketBillInfoDTO
    {
        public string movieName;
        private string _showtimeInfo;

        public string ShowtimeInfo
        {
            get {
                if (string.IsNullOrEmpty(_showtimeInfo))
                {
                    _showtimeInfo = $"{ShowDate.ToString("dd/MM/yyyy")} - {StartShowTime.ToString(@"hh\:mm")}";
                }
                return _showtimeInfo;
            }
        }                                                                                           
        public DateTime ShowDate;
        public TimeSpan StartShowTime;

        //Seat
        public List<string> seats;
        private string _SeatInfo;
        public string SeatInfo
        {
            get
            {
                if (string.IsNullOrEmpty(_SeatInfo))
                {
                    _SeatInfo = string.Join(", ", seats);
                }
                return _SeatInfo;
            }
        }

        public string RoomName
        {
            get { return $"0{roomId}"; }
        }

        public int roomId;

        public decimal TotalPriceTicket;

    }
}
