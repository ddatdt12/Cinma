using System;
using System.Collections.Generic;


namespace CinemaManagement.DTOs
{
    public class VoucherReleaseDTO
    {
        public string Id { get; set; }
        public string ReleaseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public bool Status { get; set; }
        public int ParValue { get; set; }
        private decimal _MinimumOrderValue;

        public decimal MinimumOrderValue
        {
            get { return decimal.Truncate(_MinimumOrderValue); }
            set { _MinimumOrderValue = value; }
        }
        public string ObjectType { get; set; }
        public bool EnableMerge { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public int VCount { get; set; }
        public int UnusedVCount { get; set; }

        public IList<VoucherDTO> Vouchers { get; set; }
    }
}
