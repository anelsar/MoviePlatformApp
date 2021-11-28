using APP.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace APP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public AdminController()
        {
        }
        public AdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        public ActionResult ManageUsers()
        {
            List<ManageUserViewModel> listOfUsers = new List<ManageUserViewModel>();
            foreach (var user in UserManager.Users)
                listOfUsers.Add(new ManageUserViewModel(user));
            return View(listOfUsers);
        }

        public async Task<ActionResult> UserDetails(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            ManageUserViewModel model = new ManageUserViewModel(user);

            return View(model);
            
        }
        public async Task<ActionResult> AddRoleToUser(string id)
        {
            
            var user = await UserManager.FindByIdAsync(id);
            ManageUserViewModel model = new ManageUserViewModel(user);
            
            List<RoleViewModel> list = new List<RoleViewModel>();
            foreach (var role in RoleManager.Roles)
                list.Add(new RoleViewModel(role));

            var rolesAddedToUser = await UserManager.GetRolesAsync(user.Id);
            AddRoleToUserViewModel userRole = new AddRoleToUserViewModel
            {
                singleUser = model,
                roles = list,
                userRoles = new List<string>()
            };
            await UserManager.AddToRoleAsync(user.Id, list[1].Name);
            if(rolesAddedToUser.Count > 0)
            {
                foreach (var role in rolesAddedToUser)
                    userRole.userRoles.Add(role);
            }

            return View("AddRoleToUser", userRole);
        }
    }
}