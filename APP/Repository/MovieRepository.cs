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
        private ApplicationDbContext _context;
        
        public MovieRepository()
        {
            this._context = Factory.Factory.CreateContext();
        }

        // delete a movie from database
        public void DeleteMovie(string movieId)
        {
            var movie = _context.Movies.Find(movieId);
            _context.Movies.Remove(movie);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        // get a single movie from database 
        public Movie GetMovieById(string movieId)
        {
            return _context.Movies.Find(movieId);
        }

        // get all movies from database
        public IEnumerable<Movie> GetMovies()
        {
            return _context.Movies.ToList();
        }

        // adding a movie to database
        public void InsertMovie(Movie movie)
        {
            if(movie!=null)
            {
                _context.Movies.Add(movie);
                _context.SaveChanges();
            }
        }

        // update a movie 
        public void UpdateMovie(Movie movie)
        {
            _context.Entry(movie).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}