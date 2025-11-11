using CinemaSystem.Data;
using CinemaSystem.Models;
using CinemaSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        readonly private AppDbContext _context;
        public MovieController(AppDbContext context)
        {
            _context = context;
        }
        [Route("Admin/Movie/Index/{CinemaId}")]
        public IActionResult Index(int CinemaId)
        {
            ViewBag.CinemaId = CinemaId;
            var movies = _context.Movies
                .Where(m => m.CinemaId == CinemaId)
                .AsEnumerable();
            ViewBag.CinemaId = CinemaId;
            return View(movies);
        }
        [HttpGet("admin/movie/create/{cinemaid}")]
        public IActionResult Create(int cinemaid)
        {
            ViewBag.CinemaId = cinemaid;
            var Category = _context.Categories.AsEnumerable();
            var Actor = _context.Actors.AsEnumerable();
            ActorCategoryVm vm = new ActorCategoryVm()
            {
                Actors = Actor,
                Categories = Category
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Create([FromForm]Movie movie, IFormFile Img, List<IFormFile> SubImgs,List<int> actors_id,int CinemaId)
        {
            movie.CinemaId = CinemaId;
            if (Img is not null && Img.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Img.FileName);

                // Save Img in wwwroot
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie_images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    Img.CopyTo(stream);
                }
                movie.MainImg = fileName;
            }

            _context.Movies.Add(movie);
            _context.SaveChanges();

            if (SubImgs.Count > 0)
            {
                foreach (var item in SubImgs)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie_images\\sub_images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        item.CopyTo(stream);
                    }

                    _context.MovieSubImgs.Add(new()
                    {
                        Img = fileName,
                        MovieId = movie.Id,
                    });
                }

            }
            if(actors_id.Count > 0)
            {

                foreach (var item in actors_id)
                {
                    _context.MovieActors.Add(new()
                    {
                        ActorId = item,
                        MovieId = movie.Id,
                    });
                }

                _context.SaveChanges();
            }

            TempData["success-notification"] = "Add Movie Successfully";
            return RedirectToAction(nameof(Index), new { CinemaId = movie.CinemaId });
        }

        [HttpGet("admin/movie/edit/{cinemaid}")]
        public IActionResult Edit(int cinemaid,int id)
        {
            ViewBag.CinemaId = cinemaid;
            var movie = _context.MovieActors.FirstOrDefault(e => e.MovieId == id);
            if (movie == null)
                return NotFound();
            return View(movie);
        }


        [Route("admin/movie/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.FirstOrDefault(e => e.Id == id);

            if (movie is null)
                return NotFound();

            // Delete Old Img from wwwroot
            if(movie.MainImg is not null)
            {
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie_images", movie.MainImg);

            if (System.IO.File.Exists(oldFilePath))
                System.IO.File.Delete(oldFilePath);
            }


            var movieSubImg = _context.MovieSubImgs.AsNoTracking().Where(e => e.MovieId == id).AsEnumerable();
            if(movie.SubImages is not null)
            {
            foreach (var item in movieSubImg)
            {
                var oldSubImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie_images\\sub_images", item.Img);

                if (System.IO.File.Exists(oldSubImgPath))
                    System.IO.File.Delete(oldSubImgPath);

                _context.MovieSubImgs.Remove(item);
            }
            }

            _context.Movies.Remove(movie);
            _context.SaveChanges();
            TempData["success-notification"] = "Delete Movie Successfully";
            return RedirectToAction(nameof(Index), new { CinemaId = movie.CinemaId });
        }
    }
}
