using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using luis_beuth.Models;
using luis_beuth.Models.Data;
using luis_beuth.Services;
using luis_beuth.Data;
using Microsoft.AspNetCore.Authorization;

namespace luis_beuth.Controllers
{
    public class TeacherController : Controller
    {
        private ApplicationDbContext _context;
        public TeacherController (ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: Teachers
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await this._context.Teacher.ToListAsync());
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Teacher teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this._context.Add(teacher);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(teacher);
        }

        // GET: Teacher/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await this._context.Teacher.SingleAsync(p => p.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    this._context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.TeacherExists(teacher.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var teacher = await _context.Teacher.SingleOrDefaultAsync(p => p.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }
        
        // POST: Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.SingleOrDefaultAsync(p => p.Id == id);
            this._context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        private bool TeacherExists(int id)
        {
            return this._context.Teacher.Any(p => p.Id == id);
        }
    }
}