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
        void InsertMovie(Movie movie);
        void UpdateMovie(Movie movie);
        void DeleteMovie(string movieId);

    }
}
