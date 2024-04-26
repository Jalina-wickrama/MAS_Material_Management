using System.ComponentModel.DataAnnotations;

namespace MASMaterialMgt.Models
{
    public class MaterialDto
    {
        public IFormFile? ImageFile { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = "";

        [Required, MaxLength(100)]
        public string Description { get; set; } = "";

        [Required]
        public decimal Unit_Price { get; set; }

        [Required]
        public decimal Qty { get; set; }

        [Required, MaxLength(50)]
        public string Supplier { get; set; } = "";
    }
}
