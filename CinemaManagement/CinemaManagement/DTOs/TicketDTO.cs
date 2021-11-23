using System;
using System.Collections.Generic;

namespace CinemaManagement.DTOs
{
    public class TicketDTO
    {
        public TicketDTO()
        {
        }
        public int Id { get; set; }
        public int ShowtimeId { get; set; }
        public int SeatId { get; set; }
        public decimal Price { get; set; }

        //Use when show bill details 
        public int SeatPosition { get; set; }
    }
}
