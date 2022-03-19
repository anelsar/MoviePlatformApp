using APP.Repository;
using System.Web.Mvc;
using APP.Models;
using APP.Factory;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Net;
using System.Net.Http;
using APP.Services;
using System.Collections.Generic;
using System.IO;

namespace APP.Controllers
{
    [Services.Authorize]
    public class MovieController : Controller
    {
        private readonly IMovieRepository _moviesRepo;
        private  readonly IUserMovieRepository _userMovieRepo;
        private Movie _movie;
        private  readonly IFile _imageFile;
        private  readonly IProcessFile _processFile;
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

        [Services.Authorize(Roles = "Admin, Global administrator")]
        public ActionResult Index()
        {
            if(TempData["Added"] != null)
            {
                TempData["Success"] = "Added Successfully!";
            }
            var allMovies = _moviesRepo.GetMovies(); // getting all movies from database
            return View("Index", allMovies); // sending the list of movies to view
        }

        [Services.Authorize(Roles = "Admin, Global administrator")]
        // create movie view
        public ActionResult Create()
        {
            return View();
        }

        [Services.Authorize(Roles = "Admin, Global administrator")]
        // creating a new movie
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(CreateMovieModel movieModel)
        {
            if(!ModelState.IsValid)
            {
                return View(movieModel);
            }
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
                
                if (_processFile.IsValidSignature(System.IO.Path.GetExtension(movieModel.PostedFile.FileName).Substring(1), movieModel.PostedFile.InputStream))
                {
                    var fileNameAndExtension = _processFile.GetSafeFileName(movieModel.PostedFile);
                    _imageFile.FileName = _processFile.HtmlEncodeFileName(fileNameAndExtension.Item1) + "." + _processFile.HtmlEncodeFileName(fileNameAndExtension.Item2);
                    _imageFile.PsyhicalPath = _processFile.SetPsyhicalPath(movieModel.PostedFile, _imageFile.FileName); // sets the psyhical path of the image
                    movieModel.PostedFile.SaveAs(_imageFile.PsyhicalPath); // saves image in Images directory
                    _movie.MovieImagePath = _imageFile.FileName;
                }
                // processing the uploaded image
                
                var inserted = _moviesRepo.InsertMovie(_movie); // inserting movie into database
                if(inserted == 0) // if the movie was successfully added to the DB, this number will be 1
                {
                    return RedirectToAction("MovieError", "Error");
                }
                //string added = "Added successfully";
                TempData["Added"] = "Added Successfully!"; // successfully added
            }
           
            return RedirectToAction("Index"); // redirect to action
        }

        
        [Services.Authorize(Roles = "Admin, Global administrator")]
        // edit movie view
        public ActionResult Edit(string id)
        {
            if(id != null)
            {
                _movie = _moviesRepo.GetMovieById(id); // gets a movie by id
               // var status = new HttpResponseMessage(HttpStatusCode.NotFound);
                if(_movie == null)
                {
                    
                    return RedirectToAction("MovieError", "Error");
                }
                return View(_movie);
            }
            else
            {
                return RedirectToAction("MovieError", "Error");
            }
        }

        
        [Services.Authorize(Roles = "Admin, Global administrator")]
        // editing movie information
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Movie movie)
        {
            if(!ModelState.IsValid)
            {
                return View(movie);
            }
            var updatedMovie = _moviesRepo.UpdateMovie(movie); // updates the movie, if everyting OK, updatedMovie = 1
            if(updatedMovie == 0) // error while editing the movie
            {
                return RedirectToAction("MovieError", "Error");
            }
            TempData["Success"] = "Updated successfully"; // everything OK message
            return RedirectToAction("Index");
        }

        [Services.Authorize(Roles = "Admin, Global administrator")]
        // delete movie view
        public ActionResult Delete(string id)
        {
            if(id != null)
            {
                _movie = _moviesRepo.GetMovieById(id); // getting the movie object to delete 
                if(_movie == null) // if no movie was found, show error message
                {
                    return RedirectToAction("MovieError", "Error");
                }
                return View(_movie); // show the movie on the view
            }

            return RedirectToAction("MovieError", "Error");
        }

        //deleting a movie
        [Services.Authorize(Roles = "Admin, Global administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Movie movie)
        {
            if(!ModelState.IsValid) // if a movie object was not passed to the method
            {
                return RedirectToAction("MovieError", "Error");
            }

            // getting all users with a favourite movie with this movie id
            var getAllUserMovieRecordsWithThisMovieId = _userMovieRepo.GetAllUsersWithTheSameMovie(movie.Id);

            if(getAllUserMovieRecordsWithThisMovieId != null)
            {
                List<int> deletedUserMovieRecords = new List<int>();
                // deleting those records from the UserMovies table
                foreach(var userMovieRecord in getAllUserMovieRecordsWithThisMovieId)
                {
                  _userMovieRepo.DeleteMovie(userMovieRecord.Id); // deleting all records from the userMovie table with this movie id
                                                                     // before deleting the movie from the movie table
                }
            }
            // when the records in the usermovie table are deleted, then delete the movie
            var deletedMovie = _moviesRepo.DeleteMovie(movie.Id); // if the movie was successfully deleted, deletedMovie will be equal to 1

            if(deletedMovie == 0) // movie wasn't successfully deleted, show error
            {
                return RedirectToAction("MovieError", "Error");
            }
            TempData["Success"] = "Successfully deleted";
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
            return RedirectToAction("MovieError", "Error");

        }

        // gets called after the add to favourite button on details view
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Details(string movieId, string userName)
        {           
            if(movieId != null && userName != null) // checking if we have the required parameters
            {
                var user = await UserManager.FindByEmailAsync(userName); // getting the user by his username (usernames are also unique like id's)

                if (user == null) // checking if the user exists
                {
                    return RedirectToAction("MovieError", "Error");
                }

                if (_userMovieRepo.CheckIfAlreadyFavoruite(movieId, user.Id)) // checking if the movie is already added to favourites
                {
                    TempData["Error"] = "Movie already added in favourite list";
                    return RedirectToAction("Details");
                }

                else // if movie isn't in favourites, add it
                {
                    _userMovie.MovieId = movieId;
                    _userMovie.UserId = user.Id;
                    var addedMovie =  _userMovieRepo.AddNewFavouriteMovie(_userMovie);
                    if(addedMovie == 0) // if movie was successfully added to the database, this number will be equal to 1
                    {
                        return RedirectToAction("MovieError", "Error");
                    }
                }
                TempData["Success"] = "Movie added to favourites"; // movie successfully added message
                return RedirectToAction("Details");
            }
               
            else
                return RedirectToAction("MovieError", "Error");

        }

        
        public  ActionResult FavouriteMovies(string id)
        {
            if(id != null) 
            {
                var userMovies = _userMovieRepo.GetAllUsersMovies(id); 
                return View(userMovies);
            }
            return RedirectToAction("MovieError", "Error");
        }

        
        // Deletes selected movie from favourites
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFavouriteMovie(string id)
        {
            if(id != null) 
            {
                var deletedUserMovie = _userMovieRepo.DeleteMovie(id); // if movie was successfully deleted this value will be equal to 1
                if(deletedUserMovie == 0) // something went wrong with deleting the movie
                {
                    return RedirectToAction("MovieError", "Error");
                }
                TempData["Success"] = "Movie successfully deleted from favourites"; // movie was successfully deleted from favs
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("MovieError", "Error");
        }

    }

}
