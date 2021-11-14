using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APP.Models
{
    public class CreateMovieModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public double Rating { get; set; }
        [Required]
        public string StreamingLink { get; set; }
        [Required]
        public string ImagePath { get; set; }
        [Required]
        public string Actors { get; set; }
    }
}