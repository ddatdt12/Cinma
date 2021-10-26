using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.DTOs
{
    public class ShowtimeDTO
    {
        public ShowtimeDTO()
        {
        }

        public int Id { get; set; }
        public int MovieId { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime ShowDate { get; set; }
        public int RoomId { get; set; }
        public  MovieDTO Movie { get; set; }
        public  IList<TicketDTO> Tickets { get; set; }
    }
}
