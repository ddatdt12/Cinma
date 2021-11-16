using System;
using System.Collections.Generic;

namespace CinemaManagement.DTOs
{
    public class BillDTO
    {
        public BillDTO()
        {
        }

        public string Id { get; set; }

        //Customer
        private string _CustomerId;

        public string CustomerId
        {
            get
            {
                if (_CustomerId is null)
                {
                    return "KH0000";
                }
                return _CustomerId;
            }
            set
            {
                _CustomerId = value;
            }
        }
        private string _CustomerName;
        public string CustomerName
        {
            get
            {
                if (_CustomerName is null)
                {
                   return "Khách vãng lai";
                }
                return _CustomerName; }
            set
            {
                _CustomerName = value;
            }
        }
        private string _PhoneNumber;
        public string PhoneNumber
        {
            get
            {
                if (_PhoneNumber is null)
                {
                    return "0111111111";
                }
                return _PhoneNumber;
            }
            set
            {
                _PhoneNumber = value;
            }
        }

        //Staff
        public string StaffId { get; set; }
        public string StaffName { get; set; }

        //Price
        public decimal OriginalTotalPrice { get => TotalPrice - DiscountPrice; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        //Use 2 list when show details Bill
        public List<ProductBillInfoDTO> ProductBillInfoes { get; set; }
        public TicketBillInfoDTO TicketInfo { get; set; }
    }
}
