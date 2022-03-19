using APP.Models;
using APP.Repository;
using APP.Services;
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
        [RequireHttps]
        public ActionResult Index()
        {
            return View(_movieRepository.GetMovies());
        }

        // get
        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult About(AboutViewModel model)
        {

            TempData["Message"] = "Message sent";
            ModelState.Clear();
            return View();
        }
    }
}