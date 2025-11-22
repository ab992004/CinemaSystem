using CinemaSystem.Data;
using CinemaSystem.Models;
using CinemaSystem.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CinemaSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        private readonly IRepository<Cinema> _context;
        public CinemaController(IRepository<Cinema> context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var Cinemas = await _context.GetAsync(tracked: false);
            return View(Cinemas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Cinema Cinema)
        {
            if (!ModelState.IsValid)
                return View(Cinema);

            await _context.CreateAsync(Cinema);
            await _context.CommitAsync();
            TempData["success-notification"] = "Add Cinema Successfully";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task <IActionResult> Edit(int id)
        {
            var Cinema = await _context.GetOneAsync(e => e.Id == id, tracked: true);
            if (Cinema is null)
                return NotFound();

            return View(Cinema);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cinema Cinema)
        {
            if (!ModelState.IsValid)
                return View(Cinema);

            _context.Update(Cinema);
            await _context.CommitAsync();
            TempData["success-notification"] = "Update Cinema Successfully";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var Cinema = await _context.GetOneAsync(e => e.Id == id, tracked: true);

            if (Cinema is null)
                return NotFound();

            _context.Delete(Cinema);
            await _context.CommitAsync();
            TempData["success-notification"] = "Delete Cinema Successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
