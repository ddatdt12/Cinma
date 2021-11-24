using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.DTOs
{
    public class VoucherReleaseDTO
    {
        public string Id { get; set; }
        public string ReleaseName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public string Status { get; set; }
        public string DiscountType { get; set; }
        public int ParValue { get; set; }
        public decimal MinimumOrderValue { get; set; }
        public string ObjectType { get; set; }
        public int MaximumDiscount { get; set; }
        public bool EnableMerge { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        
        public IList<VoucherDTO> Vouchers { get; set; }
    }
}
