using lab3.Data;
using lab3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace lab3.Controllers
{
    public class StudentsController : Controller
    {
        private readonly lab3Context _context;

        public StudentsController(lab3Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var context = _context;
            if(context == null)
            {
                return NotFound("No students found.");
            }

            return View(await _context.Student.Include(s => s.Course).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            var student = await _context
                .Student
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == id);

            if(id == null || _context.Student == null || student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,CourseId")] Student student)
        {
            if (student.Id != null & student.FullName != null & student.CourseId != null)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", student.CourseId);
            return View(student);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var student = await _context.Student.FindAsync(id);

            if(student == null)
            {
                return NotFound();
            }

            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", student.CourseId);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,CourseId")] Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            if (student.Id != null & student.FullName != null & student.CourseId != null)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", student.CourseId);
            return View(student);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var student = await _context.Student.Include(s => s.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                _context.Student.Remove(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
