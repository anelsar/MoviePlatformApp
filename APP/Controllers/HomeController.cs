using APP.Repository;
using System.Web.Mvc;

namespace APP.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        
        public HomeController()
        {
            _movieRepository = Factory.Factory.CreateMovieRepository();
        }
        public ActionResult Index()
        {
            var movies = _movieRepository.GetMovies();
            return View(movies);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}