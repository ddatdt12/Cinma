using System.Collections.Generic;

namespace CinemaManagement.DTOs
{
    public class CustomerDTO
    {
        public CustomerDTO()
        {
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        //public virtual IList<BillDTO> Bills { get; set; }
    }
}
