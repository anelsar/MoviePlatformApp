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
        public ActionResult Index()
        {
            return View(_movieRepository.GetMovies());
        }

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