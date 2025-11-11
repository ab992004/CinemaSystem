using CinemaSystem.Data;
using CinemaSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        private readonly AppDbContext _context;
        public CinemaController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var Cinemas = _context.Cinemas.AsNoTracking().AsEnumerable();
            return View(Cinemas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Cinema Cinema)
        {
            if (!ModelState.IsValid)
                return View(Cinema);

            _context.Cinemas.Add(Cinema);
            _context.SaveChanges();

            TempData["success-notification"] = "Add Cinema Successfully";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Cinema = _context.Cinemas.FirstOrDefault(e => e.Id == id);

            if (Cinema is null)
                return NotFound();

            return View(Cinema);
        }

        [HttpPost]
        public IActionResult Edit(Cinema Cinema)
        {
            if (!ModelState.IsValid)
                return View(Cinema);

            _context.Cinemas.Update(Cinema);
            _context.SaveChanges();

            TempData["success-notification"] = "Update Cinema Successfully";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var Cinema = _context.Cinemas.FirstOrDefault(e => e.Id == id);

            if (Cinema is null)
                return NotFound();

            _context.Cinemas.Remove(Cinema);
            _context.SaveChanges();

            TempData["success-notification"] = "Delete Cinema Successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
