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
        int AddNewFavouriteMovie(UserMovie userMovie);
        int DeleteMovie(string userMovieid);
        bool CheckIfAlreadyFavoruite(string movieId, string userId);
        IEnumerable<UserMovie> GetAllUsersWithTheSameMovie(string movieId);
    }
}
