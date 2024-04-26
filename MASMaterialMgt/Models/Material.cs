using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MASMaterialMgt.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string ImageFileName { get; set; } = "";

        [MaxLength(50)]
        public string Name { get; set; } = "";

        [MaxLength(100)]
        public string Description { get; set; } = "";

        [Precision(16, 2)]
        public decimal Unit_Price { get; set; }

        [Precision (18, 2)]
        public decimal Qty { get; set;}

        [MaxLength(50)]
        public string Supplier { get; set; } = "";
        public DateTime CreatedAt { get; set; }

    }
}
