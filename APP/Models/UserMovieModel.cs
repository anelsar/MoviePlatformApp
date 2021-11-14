using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APP.Models
{
    public class UserMovie
    {
        public string Id { get; set; }
        public string MovieId { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Movie Movie { get; set; }
    }
}