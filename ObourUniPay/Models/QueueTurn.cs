using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obour_Uni_Pay.Models
{
    public class QueueTurn
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        public int TurnNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public TurnStatus Status { get; set; } = TurnStatus.Pending;
    }
}
