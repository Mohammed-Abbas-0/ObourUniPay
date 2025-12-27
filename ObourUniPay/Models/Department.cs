using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Obour_Uni_Pay.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
