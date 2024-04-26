using System.ComponentModel.DataAnnotations;

namespace MASMaterialMgt.Models
{
    public class SupplierDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = "";

        [Required, MaxLength(50)]
        public string Address { get; set; } = "";

        [Required, MaxLength(10)]
        public string Phone { get; set; } = "";

        [MaxLength(50)]
        public string Website { get; set; } = "";

        [Required, MaxLength(10)]
        public string Activity { get; set; } = "";
    }
}
