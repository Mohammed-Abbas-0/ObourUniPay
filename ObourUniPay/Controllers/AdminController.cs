using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObourUniPay.Core.Data;
using ObourUniPay.Core.Models;

namespace Obour_Uni_Pay.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var turns = await _context.QueueTurns
                .Include(t => t.Student)
                    .ThenInclude(s => s!.Department)
                .Where(t => t.CreatedAt >= today)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return View(turns);
        }

        public async Task<IActionResult> Students()
        {
            var students = await _context.Students
                .Include(s => s.Department)
                .ToListAsync();
            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> CreateStudent()
        {
            ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Students));
            }
            ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name", student.DepartmentId);
            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> EditStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name", student.DepartmentId);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Students));
            }
            ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name", student.DepartmentId);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Students));
        }

        [HttpGet]
        public async Task<IActionResult> FixData()
        {
            var students = await _context.Students.Where(s => s.DepartmentId == null).ToListAsync();
            foreach (var s in students)
            {
                s.DepartmentId = 1; // هندسة الحاسبات
                s.Stage = "المرحلة الرابعة";
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Students));
        }
    }
}
