using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obour_Uni_Pay.Models
{
    public class QueueTicket
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        public int TicketNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsProcessed { get; set; } = false;
    }
}
