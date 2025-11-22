using CinemaSystem.Data;
using CinemaSystem.Models;
using CinemaSystem.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CinemaSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private readonly IRepository<Actor> _context;
        public ActorController(IRepository<Actor> context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //var Actors = _context.Actors.AsNoTracking().AsEnumerable();
            var Actors = await _context.GetAsync(tracked: false);

            return View(Actors);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Actor Actor, IFormFile file)
        {
            if (file is not null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Actor_images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                Actor.Img = fileName;
            }

            //_context.Actors.Add(Actor);
            //_context.SaveChanges();
            await _context.CreateAsync(Actor);
            await _context.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var actor = _context.Actors.FirstOrDefault(e => e.Id == id);
            var actor = await _context.GetOneAsync(e => e.Id == id, tracked: true);

            if (actor is null)
                return NotFound();

            return View(actor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Actor actor, IFormFile? file)
        {
            //var actorInDb = _context.Actors.FirstOrDefault(e => e.Id == actor.Id);
            var actorInDb =await _context.GetOneAsync(e => e.Id == actor.Id, tracked: true);

            if (actorInDb is null)
                return NotFound();

            if (file is not null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // Save Img in wwwroot
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Actor_images", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Actor_images", actorInDb.Img);

                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);
                actor.Img = fileName;
            }
            else
            {
                actor.Img = actorInDb.Img;
            }

            //_context.Actors.Update(actor);
            //_context.SaveChanges();
            _context.Update(actor);
            await _context.CommitAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            //var actor = _context.Actors.FirstOrDefault(e => e.Id == id);
            var actor = await _context.GetOneAsync(e => e.Id == id, tracked: true);

            if (actor is null)
                return NotFound();

            // Delete Old Img from wwwroot
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Actor_images", actor.Img);

            if (System.IO.File.Exists(oldFilePath))
                System.IO.File.Delete(oldFilePath);

            //_context.Actors.Remove(actor);
            //_context.SaveChanges();
            _context.Delete(actor);
            await _context.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
