
namespace CinemaManagement.DTOs
{
    public class SeatSettingDTO
    {
        public SeatSettingDTO()
        {
        }
        public int SeatId { get; set; }
        public int ShowtimeId { get; set; }
        public bool Status { get; set; }
        public  SeatDTO Seat { get; set; }
        public string SeatPosition
        {
            get
            {
                return $"{Seat.Row}{Seat.SeatNumber}";
            }
        }
    }
}
