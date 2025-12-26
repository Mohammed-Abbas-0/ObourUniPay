using Microsoft.AspNetCore.Mvc;
using Obour_Uni_Pay.Services;
using Obour_Uni_Pay.Models;

namespace Obour_Uni_Pay.Controllers
{
    public class QueueController : Controller
    {
        private readonly IQueueService _queueService;

        public QueueController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetTurn(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
            {
                ViewBag.Error = "Please scan a barcode.";
                return View("Index");
            }

            try
            {
                var turn = await _queueService.GenerateTurnAsync(barcode);
                return RedirectToAction("Ticket", new { turnId = turn.Id });
            }
            catch (ArgumentException ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index");
            }
            catch (Exception ex)
            {
                // Log generic error
                ViewBag.Error = "An error occurred. Please try again.";
                return View("Index");
            }
        }

        public async Task<IActionResult> Ticket(int turnId)
        {
            var turn = await _queueService.GetTurnByIdAsync(turnId);
            if (turn == null)
            {
                return RedirectToAction("Index");
            }

            return View(turn);
        }
    }
}
