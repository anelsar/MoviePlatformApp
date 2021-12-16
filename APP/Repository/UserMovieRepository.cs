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

        public void AddNewFavouriteMovie(UserMovie userMovie)
        {
            if(userMovie != null)
            {
                userMovie.Id = IdGenerate.generateId();
                _context.UserMovies.Add(userMovie);
                _context.SaveChanges();
            }
        }

        public bool CheckIfAlreadyFavoruite(string movieId, string userId)
        {
            if(movieId != null && userId != null)
            {
                return _context.UserMovies.Where(x => x.UserId == userId && x.MovieId == movieId).Any();
            }
            throw new Exception();
        }

        public void DeleteMovie(string userMovieid)
        {
            if(userMovieid != null)
            {
                var movie = _context.UserMovies.Find(userMovieid);
                _context.UserMovies.Remove(movie);
                _context.SaveChanges();
            }
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