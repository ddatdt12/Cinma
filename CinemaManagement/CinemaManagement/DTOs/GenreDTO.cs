using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.DTOs
{
    public class GenreDTO
    {
        public GenreDTO()
        {
        }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public IList<MovieDTO> Movies{ get; set; }
    }
}
