using APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Repository
{
    public interface IMovieRepository : IDisposable
    {
        IEnumerable<Movie> GetMovies();
        Movie GetMovieById(string movieId);
        int InsertMovie(Movie movie);
        int UpdateMovie(Movie movie);
        int DeleteMovie(string movieId);

    }
}
