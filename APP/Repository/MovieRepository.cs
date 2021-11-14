using APP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace APP.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private ApplicationDbContext context;
        public MovieRepository(ApplicationDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteMovie(string movieId)
        {
            Movie movie = context.Movies.Find(movieId);
            context.Movies.Remove(movie);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Movie GetMovieById(string movieId)
        {
            return context.Movies.Find(movieId);
        }

        public IEnumerable<Movie> GetMovies()
        {
            return context.Movies.ToList();
        }

        public void InsertMovie(Movie movie)
        {
            if(movie!=null)
            {
                context.Movies.Add(movie);
                context.SaveChanges();
            }
        }

        public void UpdateMovie(Movie movie)
        {
            context.Entry(movie).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}