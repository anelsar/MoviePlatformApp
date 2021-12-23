using APP.Factory;
using APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APP.Repository
{
    public class UserMovieRepository : IUserMovieRepository
    {
        private readonly ApplicationDbContext _context;
        
        public UserMovieRepository()
        {
            //home made dependency injection
            this._context = Factory.Factory.CreateContext();
        }

        public int AddNewFavouriteMovie(UserMovie userMovie)
        {
            if(userMovie != null)
            {
                userMovie.Id = IdGenerate.generateId();
                _context.UserMovies.Add(userMovie);
                var addedMovie = _context.SaveChanges();
                return addedMovie;
            }
            return 0;
        }

        public bool CheckIfAlreadyFavoruite(string movieId, string userId)
        {          
                return _context.UserMovies.Where(x => x.UserId == userId && x.MovieId == movieId).Any();   
        }

        public int DeleteMovie(string userMovieid)
        {
            if(userMovieid != null)
            {
                var movie = _context.UserMovies.Find(userMovieid);
                if (movie == null)
                    return 0;
                _context.UserMovies.Remove(movie);
                var deletedUserMovie = _context.SaveChanges();
                return deletedUserMovie;
            }
            return 0;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserMovie> GetAllUsersMovies(string userId)
        {
            return _context.UserMovies.Where(x => x.UserId == userId).ToList();
        }

        
    }
}