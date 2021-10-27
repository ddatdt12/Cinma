using System;

namespace CinemaManagement.DTOs
{
    public class SeatDTO
    {
        public SeatDTO()
        {
        }

        public int Id { get; set; }
        public int SeatNumber { get; set; }
        public string Row { get; set; }
        public Nullable<int> RoomId { get; set; }

        public RoomDTO Room { get; set; }
    }
}
