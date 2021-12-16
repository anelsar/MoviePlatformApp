using APP.Repository;
using System.Web.Mvc;
using APP.Models;
using APP.Factory;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace APP.Controllers
{
    [Services.Authorize]
    public class MovieController : Controller
    {
        private  IMovieRepository _moviesRepo;
        private  IUserMovieRepository _userMovieRepo;
        private Movie _movie;
        private  IFile _imageFile;
        private  IProcessFile _processFile;
        private  UserMovie _userMovie;
        private ApplicationUserManager _userManager;

        public MovieController()
        {
            this._moviesRepo = Factory.Factory.CreateMovieRepository(); // getting MovieRepository instance
            this._movie = Factory.Factory.CreateMovieInstance();
            this._imageFile = Factory.Factory.CreateFile();
            this._processFile = Factory.Factory.CreateProcessFile();
            this._userMovieRepo = Factory.Factory.CreateUserMovieRepository();
            this._userMovie = Factory.Factory.CreateUserMovieInstance();
        }
        public MovieController(ApplicationUserManager userManager)
        {
            this._moviesRepo = Factory.Factory.CreateMovieRepository(); // getting MovieRepository instance
            this._movie = Factory.Factory.CreateMovieInstance();
            this._imageFile = Factory.Factory.CreateFile();
            this._processFile = Factory.Factory.CreateProcessFile();
            this._userMovieRepo = Factory.Factory.CreateUserMovieRepository();
            this._userMovie = Factory.Factory.CreateUserMovieInstance();
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        public ActionResult Index()
        {
            var allMovies = _moviesRepo.GetMovies(); // getting all movies from database
            return View("Index", allMovies); // sending the list of movies to view
        }
        [Services.Authorize(Roles = "Admin")]
        // create movie view
        public ActionResult Create()
        {
            return View();
        }
        [Services.Authorize(Roles = "Admin")]
        // creating a new movie
        [HttpPost]
        public ActionResult Create(CreateMovieModel movieModel)
        {
            if(ModelState.IsValid)
            {
                // assigning values to new movie
                _movie.Id = IdGenerate.generateId(); // generate unique movie id
                _movie.MovieName = movieModel.Name;
                _movie.MovieActors = movieModel.Actors;
                _movie.MovieDescription = movieModel.Description;
                _movie.MovieDuration = movieModel.Duration;
                _movie.MovieImagePath = movieModel.PostedFile.FileName; // ovdje moram image path 
                _movie.MovieRating = movieModel.Rating;
                _movie.MovieStreamingLink = movieModel.StreamingLink;

                // processing the uploaded image
                _imageFile.FileName = _processFile.GetFileName(movieModel.PostedFile); // gets the name of the image
                _imageFile.PsyhicalPath = _processFile.SetPsyhicalPath(movieModel.PostedFile, _imageFile.FileName); // sets the psyhical path of the image
                movieModel.PostedFile.SaveAs(_imageFile.PsyhicalPath); // saves image in Images directory
                _moviesRepo.InsertMovie(_movie); // inserting movie into database

                TempData["Success"] = "Added Successfully!"; // successfully added
            }
           
            return RedirectToAction("Index", "Movie"); // redirect to view
        }

        [Services.Authorize(Roles = "Admin")]
        // edit movie view
        public ActionResult Edit(string id)
        {
            _movie = _moviesRepo.GetMovieById(id); // gets a movie by id
            return View(_movie);
        }

        [Services.Authorize(Roles = "Admin")]
        // editing movie information
        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            _moviesRepo.UpdateMovie(movie); // updates the movie
            return RedirectToAction("Index");
        }

        [Services.Authorize(Roles = "Admin")]
        // delete movie view
        public ActionResult Delete(string id)
        {
            _movie = _moviesRepo.GetMovieById(id);
            return View(_movie);
        }

        [Services.Authorize(Roles = "Admin")]
        //deleting a movie
        [HttpPost]
        public ActionResult Delete(Movie movie)
        {
            _moviesRepo.DeleteMovie(movie.Id);
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        // get movie details
        public ActionResult Details(string id)
        {
            _movie = _moviesRepo.GetMovieById(id);
            return View(_movie);
        }

        // gets called after the add to favourite button on details view
        [HttpPost]

        public async Task<ActionResult> Details(string movieId, string userName)
        {           
            var user = await UserManager.FindByEmailAsync(userName); // getting the user by his username (usernames are also unique like id's)

            if (_userMovieRepo.CheckIfAlreadyFavoruite(movieId, user.Id)) // checking if the movie is already added to favourites
            {
                TempData["Error"] = "Movie already added in favourite list";
                return RedirectToAction("Details");
            }
            else // if movies isn't in favourites, add it
            {
                _userMovie.MovieId = movieId;
                _userMovie.UserId = user.Id;
                _userMovieRepo.AddNewFavouriteMovie(_userMovie);
            }        


            return RedirectToAction("Details");
        }

        
        public async Task<ActionResult> FavouriteMovies(string userName)
        {
            var user = await UserManager.FindByEmailAsync(userName);
            var userMovies = _userMovieRepo.GetAllUsersMovies(user.Id);
            return View(userMovies);
        }

        
        // Deletes selected movie from favourites
        [HttpPost]
        public ActionResult DeleteFavouriteMovie(string id)
        {
            if(id != null)
                _userMovieRepo.DeleteMovie(id);
            return RedirectToAction("Index", "Home");
        }
    }
}
