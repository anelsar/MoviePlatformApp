using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APP.Models
{
    public class AddRoleToUserViewModel
    {
        public AddRoleToUserViewModel() 
        {
            CheckedUserRoles = new List<CheckboxRole>();
        }
        public ManageUserViewModel SingleUser { get; set; } // trenutni korisnik kojeg gledamo
        public List<CheckboxRole> CheckedUserRoles { get; set; } // role, one koje pripadaju korisniku su oznacene  
    }

    public class CheckboxRole
    {
        public CheckboxRole() { }
        public CheckboxRole(RoleViewModel role, bool isChecked)
        {
            this.Role = role;
            this.IsChecked = isChecked;
        }
        public RoleViewModel Role { get; set; }
        public bool IsChecked { get; set; }
    }
}