using CinemaSystem.Data;
using CinemaSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        readonly private AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string CinemaName, int page = 1)
        {
            var Cinemas = _context.Cinemas.AsNoTracking().AsQueryable();

            #region Filter
            if(CinemaName is not null)
            {
                Cinemas = Cinemas.Where(c => c.Name.Contains(CinemaName));
                ViewBag.CinemaName = CinemaName;
            }
            #endregion


            #region Pagination

            ViewBag.totalPages = Math.Ceiling(Cinemas.Count() / 8.0);
            ViewBag.currentPage = page;
            Cinemas = Cinemas.Skip((page - 1) * 8).Take(8);

            #endregion
            return View(Cinemas.AsEnumerable());
        }
        [Route("Customer/Home/Movies/{CinemaId}")]
        public IActionResult Movies(int CinemaId,string MovieTitle, int page = 1)
        {
            var movies = _context.Movies
                .AsNoTracking()
                .Where(m => m.CinemaId == CinemaId);
            #region Filter
            if (MovieTitle is not null)
            {
                movies = movies.Where(m => m.Title.Contains(MovieTitle));
                ViewBag.MovieTitle = MovieTitle;
            }
            #endregion
            return View(movies.AsEnumerable());
        }
        [Route("Customer/Home/Movies/Details/{id}")]
        public IActionResult Details(int id)
        {
            
            var movie = _context.Movies.Include(m => m.SubImages).Include(m => m.Category)
        .Include(m => m.MovieActors)
            .ThenInclude(ma => ma.Actor)
        .AsNoTracking()
        .FirstOrDefault(m => m.Id == id);
            if (movie is null) return NotFound();
            return View(movie);
        }
    }
}
