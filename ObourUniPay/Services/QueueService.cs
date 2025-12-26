using Microsoft.EntityFrameworkCore;
using Obour_Uni_Pay.Data;
using Obour_Uni_Pay.Models;

namespace Obour_Uni_Pay.Services
{
    public class QueueService : IQueueService
    {
        private readonly ApplicationDbContext _context;

        public QueueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<QueueTurn> GenerateTurnAsync(string barcode)
        {
            // Find student
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Barcode == barcode);
            if (student == null)
            {
                throw new ArgumentException("Student not found");
            }

            // Check if student already has a pending turn today
            var today = DateTime.Today;
            var existingTurn = await _context.QueueTurns
                .FirstOrDefaultAsync(t => t.StudentId == student.Id && t.CreatedAt >= today && t.Status == TurnStatus.Pending);

            if (existingTurn != null)
            {
                return existingTurn;
            }

            // Generate new turn number
            // Reset logic is implicit: we only count turns created today
            var lastTurn = await _context.QueueTurns
                .Where(t => t.CreatedAt >= today)
                .OrderByDescending(t => t.TurnNumber)
                .FirstOrDefaultAsync();

            int nextTurnNumber = (lastTurn?.TurnNumber ?? 0) + 1;

            var newTurn = new QueueTurn
            {
                StudentId = student.Id,
                TurnNumber = nextTurnNumber,
                CreatedAt = DateTime.Now,
                Status = TurnStatus.Pending
            };

            _context.QueueTurns.Add(newTurn);
            await _context.SaveChangesAsync();

            return newTurn;
        }

        public async Task<List<QueueTurn>> GetDailyQueueAsync()
        {
            var today = DateTime.Today;
            return await _context.QueueTurns
                .Include(t => t.Student)
                .Where(t => t.CreatedAt >= today)
                .OrderBy(t => t.TurnNumber)
                .ToListAsync();
        }

        public async Task<QueueTurn?> GetTurnByIdAsync(int turnId)
        {
            return await _context.QueueTurns
                .Include(t => t.Student)
                .FirstOrDefaultAsync(t => t.Id == turnId);
        }

        public async Task<QueueTurn?> GetCurrentTurnAsync()
        {
            var today = DateTime.Today;
            return await _context.QueueTurns
                .Include(t => t.Student)
                .Where(t => t.CreatedAt >= today && t.Status == TurnStatus.Pending)
                .OrderBy(t => t.TurnNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<TurnStatus> UpdateTurnStatusAsync(int turnId, TurnStatus status)
        {
            var turn = await _context.QueueTurns.FindAsync(turnId);
            if (turn == null) throw new ArgumentException("Turn not found");

            turn.Status = status;
            await _context.SaveChangesAsync();
            return turn.Status;
        }

        public async Task ResetQueueAsync()
        {
            // In this design, reset is automatic by date filtering.
            // But if we wanted to physically delete or archive, we could do it here.
            // For now, no-op or maybe clear pending turns from yesterday?
            await Task.CompletedTask;
        }
    }
}
