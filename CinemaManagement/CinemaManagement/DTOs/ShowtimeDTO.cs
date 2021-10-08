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
        public Nullable<int> ShowtimeSettingId { get; set; }
        public Nullable<int> MovieId { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }

        public  MovieDTO Movie { get; set; }
        public virtual IList<TicketDTO> Tickets { get; set; }
    }
}
