using APP.Models;
using APP.Repository;
using APP.Services;
using System.Net.Mail;

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

        public static IUserMovieRepository CreateUserMovieRepository()
        {
            return new UserMovieRepository();
        }

        public static UserMovie CreateUserMovieInstance()
        {
            return new UserMovie();
        }

        public static SmtpClient CreateSmtpClient()
        {
            return new SmtpClient();
        }

        public static IEmailService CreateMessageService()
        {
            return new APP.Services.EmailService();
        }
    }
}