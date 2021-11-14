using APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APP.Repository
{
    public class UserMovieRepository : IUserMovieRepository
    {
        private ApplicationDbContext context;
        
        public UserMovieRepository(ApplicationDbContext _context)
        {
            this.context = _context;
        }

        public void AddNewMovie(UserMovie userMovie)
        {
            if(userMovie != null)
            {
                context.UserMovies.Add(userMovie);
                context.SaveChanges();
            }
        }

        public void DeleteMovie(string userMovieid)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserMovie> GetAllUsersMovies(string userId)
        {
            throw new NotImplementedException();
        }
    }
}