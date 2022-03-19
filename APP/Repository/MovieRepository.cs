using APP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace APP.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        
        public MovieRepository()
        {
            this._context = Factory.Factory.CreateContext();
        }

        // delete a movie from database
        public int DeleteMovie(string movieId)
        {
            if(movieId != null)
            {
                var movie = _context.Movies.Find(movieId);
                _context.Movies.Remove(movie);
                var deletedMovie = _context.SaveChanges(); 
                return deletedMovie;
            }
            return 0;
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
        public int InsertMovie(Movie movie)
        {
            if(movie!=null)
            {
                _context.Movies.Add(movie);
                var number = _context.SaveChanges();
                return number;
            }
            return 0;
        }

        // update a movie 
        public int UpdateMovie(Movie movie)
        {
            if(movie != null)
            {
                _context.Entry(movie).State = EntityState.Modified;
                var updatedMovie = _context.SaveChanges();
                return updatedMovie;
            }
            return 0;
        }
    }
}