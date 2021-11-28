using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APP.Models
{
    public class ManageUserViewModel
    {
        public ManageUserViewModel() { }
        public ManageUserViewModel(ApplicationUser user) 
        {
            Id = user.Id;
            Name = user.Email;
            //Roles = user.Roles;
        }
        public string Id { get; set; }
        public string Name { get; set; }

        //List<string> Roles { get; set; }
    }

}