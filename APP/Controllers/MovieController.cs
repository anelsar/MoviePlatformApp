using APP.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APP.Controllers
{
    public class MovieController : Controller
    {
        private MovieRepository _movies;

        public MovieController() { }
        public MovieController(MovieRepository movies)
        {
            this._movies = movies;
        }

        public ActionResult Index()
        {
            var allMovies = _movies.GetMovies();
            
            return View("Index" ,allMovies);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string id)
        {
            return View();
        }
        
        
    }
}