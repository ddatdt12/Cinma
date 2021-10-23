using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.DTOs
{
    public class MovieDTO
    {
        public MovieDTO()
        {
            MovieType = "2D";
        }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int RunningTime { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public string MovieType { get; set; }
        public string Director { get; set; }
        public IList<ShowtimeDTO> Showtimes { get; set; }
        public IList<GenreDTO> Genres { get; set; }
    }
}
