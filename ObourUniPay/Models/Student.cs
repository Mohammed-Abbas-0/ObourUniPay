using System.ComponentModel.DataAnnotations;

namespace Obour_Uni_Pay.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        [Required]
        public string Barcode { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public string? Stage { get; set; }

        public string? Gender { get; set; }
    }
}
