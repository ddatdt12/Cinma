using System.Collections.Generic;

namespace CinemaManagement.DTOs
{
    public class RoomDTO
    {
        public RoomDTO()
        {
            this.Seats = new List<SeatDTO>();
        }

        public int Id { get; set; }
        public int SeatQuantity { get; set; }
        public IList<SeatDTO> Seats { get; set; }
    }
}
