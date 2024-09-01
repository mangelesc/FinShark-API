using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class UpdateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 characters")]
        public string Symbol { get; set; } = string.Empty; 

        [Required]
        [MinLength(2, ErrorMessage = "CompanyName mus be at least 2 characters")]
        [MaxLength(60, ErrorMessage = "CompanyName cannot be over 60 characters")]
        public string CompanyName { get; set; } = string.Empty; 
        [Required]
        [Range(1, 10000000000)]
        public decimal Purchase { get; set; }
        
        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }

        [Required]
        [MaxLength(60, ErrorMessage = "CompanyName cannot be over 60 characters")]
        public string Industry { get; set; } = string.Empty;
                
        [Required]
        [Range(1, 500000000000)]
        public long MarketCap { get; set; }
    }
}