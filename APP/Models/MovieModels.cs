using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APP.Models
{
    public class Movie
    {
        
        public string Id { get; set; }
        public string MovieName { get; set; }
        public string MovieDescription { get; set; }
        public int MovieDuration { get; set; }
        [Range(1, 10)]
        public double MovieRating { get; set; }
        public string MovieStreamingLink { get; set; }
        public string MovieImagePath { get; set; }
        public string MovieActors { get; set; }

    }
}