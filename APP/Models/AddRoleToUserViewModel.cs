using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APP.Models
{
    public class AddRoleToUserViewModel
    {
        public AddRoleToUserViewModel() { }
        public ManageUserViewModel singleUser { get; set; } // trenutni korisnik kojeg gledamo
        public List<RoleViewModel> roles { get; set; } // spisak svih mogucih rola
        public List<string> userRoles { get; set; } // spisak role pridruzenih korisniku



       
    }
}