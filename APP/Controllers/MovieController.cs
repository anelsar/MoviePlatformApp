using APP.Repository;
using System.Web.Mvc;
using APP.Models;
using APP.Factory;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System;

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
        [Services.Authorize(Roles = "Admin")]
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
                _movie.MovieImagePath = movieModel.PostedFile.FileName; 
                _movie.MovieRating = movieModel.Rating;
                _movie.MovieStreamingLink = movieModel.StreamingLink;

                // processing the uploaded image
                _imageFile.FileName = _processFile.GetFileName(movieModel.PostedFile); // gets the name of the image
                _imageFile.PsyhicalPath = _processFile.SetPsyhicalPath(movieModel.PostedFile, _imageFile.FileName); // sets the psyhical path of the image
                movieModel.PostedFile.SaveAs(_imageFile.PsyhicalPath); // saves image in Images directory
                var inserted = _moviesRepo.InsertMovie(_movie); // inserting movie into database
                if(inserted == 0) // if the movie was successfully added to the DB, this number will be 1
                {
                    ViewBag.errorMessage = "Problem with adding the movie to database";
                    return View("../Shared/Error");
                }
                TempData["Success"] = "Added Successfully!"; // successfully added
            }
           
            return RedirectToAction("Index", "Movie"); // redirect to view
        }

        [Services.Authorize(Roles = "Admin")]
        // edit movie view
        public ActionResult Edit(string id)
        {
            if(id != null)
            {
                _movie = _moviesRepo.GetMovieById(id); // gets a movie by id
                if(_movie == null)
                {
                    ViewBag.errorMessage = "No movie with that ID";
                    return View("../Shared/Error");
                }
                return View(_movie);
            }
            else
            {
                ViewBag.errorMessage = "Movie ID not found";
                return View("../Shared/Error");
            }
        }

        [Services.Authorize(Roles = "Admin")]
        // editing movie information
        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            if(movie == null)
            {
                ViewBag.errorMessage = "No movie object provided for editing";
                return View("../Shared/Error");
            }
            var updatedMovie = _moviesRepo.UpdateMovie(movie); // updates the movie, if everyting OK, updatedMovie = 1
            if(updatedMovie == 0) // error while editing the movie
            {
                ViewBag.errorMessage = "Something went wrong while updating the movie information";
                return View("../Shared/Error");
            }
            TempData["Success"] = "Updated successfully"; // everything OK message
            return RedirectToAction("Index");
        }

        [Services.Authorize(Roles = "Admin")]
        // delete movie view
        public ActionResult Delete(string id)
        {
            if(id != null)
            {
                _movie = _moviesRepo.GetMovieById(id); // getting the movie object to delete 
                if(_movie == null) // if no movie was found, show error message
                {
                    ViewBag.errorMessage = "No movie with this ID";
                    return View("../Shared/Error");
                }
                return View(_movie); // show the movie on the view
            }

            ViewBag.errorMessage = "Movie ID not found";
            return View("../Shared/Error");
        }

        //deleting a movie
        [Services.Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(Movie movie)
        {
            if(movie == null) // if a movie object was not passed to the method
            {
                ViewBag.errorMessage = "Problem with providing a movie to delete";
                return View("../Shared/Error");
            }
            var deletedMovie = _moviesRepo.DeleteMovie(movie.Id); // if the movie was successfully deleted, deletedMovie will be equal to 1
            if(deletedMovie == 0) // movie wasn't successfully deleted, show error
            {
                ViewBag.errorMessage = "Problem while deleting the movie, movie wasn't deleted";
                return View("../Shared/Error");
            }
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        // get movie details
        public ActionResult Details(string id)
        {         
            _movie = _moviesRepo.GetMovieById(id); // get the movie by id
            if(_movie!=null) 
                return View(_movie);

            // if no movie with the id was found, show error 
            ViewBag.errorMessage = "No movie with that ID";
            return View("../Shared/Error");
                    
        }

        // gets called after the add to favourite button on details view
        [HttpPost]

        public async Task<ActionResult> Details(string movieId, string userName)
        {           
            if(movieId != null && userName != null) // checking if we have the required parameters
            {
                var user = await UserManager.FindByEmailAsync(userName); // getting the user by his username (usernames are also unique like id's)

                if (user == null) // checking if the user exists
                {
                    ViewBag.errorMessage = "No user with that ID";  // user doesn't exist
                    return View("../Shared/Error"); // show the error view
                }

                if (_userMovieRepo.CheckIfAlreadyFavoruite(movieId, user.Id)) // checking if the movie is already added to favourites
                {
                    TempData["Error"] = "Movie already added in favourite list";
                    return RedirectToAction("Details");
                }

                else // if movies isn't in favourites, add it
                {
                    _userMovie.MovieId = movieId;
                    _userMovie.UserId = user.Id;
                    var addedMovie =  _userMovieRepo.AddNewFavouriteMovie(_userMovie);
                    if(addedMovie == 0) // if movie was successfully added to the database, this number will be equal to 1
                    {
                        ViewBag.errorMessage = "Some problem occured while adding the movie to the database";  // movie wasn't added to usermovies db
                        return View("../Shared/Error");
                    }
                }
                TempData["Success"] = "Movie added to favourites"; // movie successfully added message
                return RedirectToAction("Details");
            }
               
            else
                ViewBag.errorMessage = "Not a valid movie id or user name"; // no valid id's were provided, show error 
            return View("../Shared/Error");

        }

        
        public  ActionResult FavouriteMovies(string id)
        {
            if(id != null) 
            {
                var userMovies = _userMovieRepo.GetAllUsersMovies(id); 
                return View(userMovies);
            }
            ViewBag.errorMessage = "Not a valid user id"; // if no id was provided show error
            return View("../Shared/Error");
        }

        
        // Deletes selected movie from favourites
        [HttpPost]
        public ActionResult DeleteFavouriteMovie(string id)
        {
            if(id != null) 
            {
                var deletedUserMovie = _userMovieRepo.DeleteMovie(id); // if movie was successfully deleted this value will be equal to 1
                if(deletedUserMovie == 0) // something went wrong with deleting the movie
                {
                    ViewBag.errorMessage = "Something went wrong while deleting the selected favourite movie"; // show error message
                    return View("../Shared/Error");
                }
                TempData["Success"] = "Movie successfully deleted from favourites"; // movie was successfully deleted from favs
                return RedirectToAction("Index", "Home");
            }
            ViewBag.errorMessage = "Not a valid movie id for"; // if no id was provided show error
            return View("../Shared/Error");
        }
    }
}
