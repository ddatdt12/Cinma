using System.Collections.Generic;

namespace CinemaManagement.DTOs
{
    public class GenreDTO
    {
        public GenreDTO()
        {
        }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public IList<MovieDTO> Movies { get; set; }

    }
}
