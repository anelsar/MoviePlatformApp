using APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Repository
{
    public interface IUserMovieRepository : IDisposable
    {
        IEnumerable<UserMovie> GetAllUsersMovies(string userId);
        void AddNewMovie(UserMovie userMovie);
        void DeleteMovie(string userMovieid);
    }
}
