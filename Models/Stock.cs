using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Stock
    {
        public int Id { get; set; }
        // string.Empty = "", not null
        public string Symbol { get; set; } = string.Empty; 
        public string CompanyName { get; set; } = string.Empty; 
        // To force the SQL db to limit it to 18 digits and 2 decimals places
        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>(); 

    }
}