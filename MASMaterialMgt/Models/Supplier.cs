using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MASMaterialMgt.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = "";

        [MaxLength(50)]
        public string Address { get; set; } = "";

        [MaxLength(10)]
        public string Phone { get; set; } = "";

        [MaxLength(50)]
        public string Website { get; set; } = "";

        [MaxLength(10)]
        public string Activity { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
