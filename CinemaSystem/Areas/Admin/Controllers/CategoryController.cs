using CinemaSystem.Data;
using CinemaSystem.Models;
using CinemaSystem.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CinemaSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        readonly IRepository<Category> _context;
        public CategoryController(IRepository<Category> context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //var categories = _context.Categories.AsNoTracking().AsQueryable();
            var categories = await _context.GetAsync(tracked: false);

            return View(categories.AsEnumerable());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            //_context.Categories.Add(category);
            //_context.SaveChanges();
            await _context.CreateAsync(category);
            await _context.CommitAsync();

            TempData["success-notification"] = "Add Category Successfully";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var category = _context.Categories.FirstOrDefault(e => e.Id == id);
            var category = await _context.GetOneAsync(e => e.Id == id, tracked: true);

            if (category is null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            //_context.Categories.Update(category);
            //_context.SaveChanges();
            _context.Update(category);
            await _context.CommitAsync();

            TempData["success-notification"] = "Update Category Successfully";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //var category = _context.Categories.FirstOrDefault(e => e.Id == id);
            var category = await _context.GetOneAsync(e => e.Id == id, tracked: true);

            if (category is null)
                return NotFound();

            //_context.Categories.Remove(category);
            //_context.SaveChanges();
            _context.Delete(category);
            await _context.CommitAsync();
            TempData["success-notification"] = "Delete Category Successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
