using APP.Models;
using APP.Repository;

namespace APP.Factory
{
    public static class Factory
    {
        // Factory for providing object instances 
        public static IMovieRepository CreateMovieRepository()
        {
            return new MovieRepository();
        }

        public static ApplicationDbContext CreateContext()
        {
            return new ApplicationDbContext();
        }

        public static Movie CreateMovieInstance()
        {
            return new Movie();
        }

        public static IFile CreateFile()
        {
            return new ImageFile();
        }

        public static IProcessFile CreateProcessFile()
        {
            return new ProcessImageFile();
        }
    }
}