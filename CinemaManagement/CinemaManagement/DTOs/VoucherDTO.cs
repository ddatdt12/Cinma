using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.DTOs
{
    public class VoucherDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string VoucherReleaseId { get; set; }
        public string Status { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Nullable<System.DateTime> UsedAt { get; set; }
        public Nullable<System.DateTime> ReleaseAt { get; set; }
    }
}
