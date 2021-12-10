using APP.Repository;
using System.Web.Mvc;
using APP.Models;
using APP.Factory;

namespace APP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MovieController : Controller
    {
        private IMovieRepository _moviesRepo;
        private Movie _movie;
        private IFile _imageFile;
        private IProcessFile _processFile;
        public MovieController()
        {
            this._moviesRepo = Factory.Factory.CreateMovieRepository(); // getting MovieRepository instance
            this._movie = Factory.Factory.CreateMovieInstance();
            this._imageFile = Factory.Factory.CreateFile();
            this._processFile = Factory.Factory.CreateProcessFile();
        }

        public ActionResult Index()
        {
            var allMovies = _moviesRepo.GetMovies(); // getting all movies from database
            return View("Index", allMovies); // sending the list of movies to view
        }

        // create movie view
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateMovieModel movieModel)
        {
            // adding new movie
            _movie.Id = IdGenerate.generateId(); // generate unique movie id
            _movie.MovieName = movieModel.Name;
            _movie.MovieActors = movieModel.Actors;
            _movie.MovieDescription = movieModel.Description;
            _movie.MovieDuration = movieModel.Duration;
            _movie.MovieImagePath = movieModel.PostedFile.FileName  ; // ovdje moram image path 
            _movie.MovieRating = movieModel.Rating;
            _movie.MovieStreamingLink = movieModel.StreamingLink;

            // processing the uploaded image
            _imageFile.FileName = _processFile.GetFileName(movieModel.PostedFile);
            _imageFile.PsyhicalPath = _processFile.SetPsyhicalPath(movieModel.PostedFile, _imageFile.FileName);
            movieModel.PostedFile.SaveAs(_imageFile.PsyhicalPath);
            _moviesRepo.InsertMovie(_movie); // inserting into DB

            TempData["Success"] = "Added Successfully!"; // successfully added
            return RedirectToAction("Index", "Movie"); // redirect to view
        }
        
        // edit movie view
        public ActionResult Edit(string id)
        {
            _movie = _moviesRepo.GetMovieById(id); // gets a movie by id
            return View(_movie);
        }
        
        // editing movie information
        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            _moviesRepo.UpdateMovie(movie);
            return RedirectToAction("Index");
        }

        // delete movie view
        public ActionResult Delete(string id)
        {
            _movie = _moviesRepo.GetMovieById(id);
            return View(_movie);
        }

        //deleting a movie
        [HttpPost]
        public ActionResult Delete(Movie movie)
        {
            _moviesRepo.DeleteMovie(movie.Id);
            return RedirectToAction("Index");
        }

        // get movie details
        public ActionResult Details(string id)
        {
            _movie = _moviesRepo.GetMovieById(id);
            return View(_movie);
        }
        [HttpPost]
        public ActionResult Details(Movie movie)
        {
            // e treba mi i user id DANTEJEBO
            return RedirectToAction("Details");
        }
    }
}