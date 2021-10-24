﻿using System.Collections.Generic;

namespace CinemaManagement.DTOs
{
    public class ProductDTO
    {
        public ProductDTO()
        {
        }

        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }

        public IList<ProductBillInfoDTO> ProductBillInfoes { get; set; }
    }
}
