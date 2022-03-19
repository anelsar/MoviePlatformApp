using APP.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace APP.Controllers
{
    [Services.Authorize(Roles = "Admin, Global administrator")]
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
            if(id != null)
            {
                var user = await UserManager.FindByIdAsync(id);
                if(user == null)
                {
                    ViewBag.errorMessage = "No user with that user ID";
                    return View("../Shared/Error");
                }
                ManageUserViewModel model = new ManageUserViewModel(user);
                return View(model);
            }
            
            ViewBag.errorMessage = "Not a valid user ID";
            return View("../Shared/Error");
        }

        // GET 
        [Services.Authorize(Roles = "Global administrator")]
        public async Task<ActionResult> AddRoleToUser(string id) // prikazuje sve role, one koje su dodijeljene trenutnom korisniku su oznacene
        {         
            var user = await UserManager.FindByIdAsync(id); // vraca nam trenutnog korisnika
            ManageUserViewModel userModel = new ManageUserViewModel(user); 
            
            List<RoleViewModel> listOfAllRoles = new List<RoleViewModel>();
            foreach (var role in RoleManager.Roles)
                listOfAllRoles.Add(new RoleViewModel(role)); // u listu stavljamo sve postojece role

            var rolesAddedToUser = await UserManager.GetRolesAsync(user.Id); // vraca nam role koje pripadaju korisniku

            AddRoleToUserViewModel userRole = new AddRoleToUserViewModel()
            {
                SingleUser = userModel
            };
            
            foreach(var role in listOfAllRoles) // lista svih postojecih rola
            {
                userRole.CheckedUserRoles.Add(new CheckboxRole(role, false)); // dodajemo sve role, u pocetku checbox pripadnosti useru oznacavno sa false
            }

            foreach(var role in rolesAddedToUser)
            {
                foreach(var checkedRole in userRole.CheckedUserRoles)
                {
                    if (role == checkedRole.Role.Name) 
                        checkedRole.IsChecked = true; // role koje su pridruzene useru oznacavamo sa true
                }
            }
            // sad fercera lijepo, moram malo doraditi da zadovoljava SOLID
            return View("AddRoleToUser", userRole);
        }

        [HttpPost]
        [Services.Authorize(Roles = "Global administrator")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRoleToUser(AddRoleToUserViewModel model)
        {
           if(model != null)
            {
                var rolesAddedToUser = await UserManager.GetRolesAsync(model.SingleUser.Id);
                if(rolesAddedToUser.Count == 0) // ako korisniku nema niti jednu rolu pridruzenu
                {
                    foreach(var role in model.CheckedUserRoles)
                    {
                        if (role.IsChecked == true)
                            await UserManager.AddToRoleAsync(model.SingleUser.Id, role.Role.Name);
                    }
                }
                else // ako korisnik vec ima neku rolu pridruzenu
                {
                    foreach (var role in model.CheckedUserRoles)
                    {
                        foreach (var alreadyAsignedRole in rolesAddedToUser)
                        {
                            if (alreadyAsignedRole != role.Role.Name && role.IsChecked == true) // ako rola nije pridruzena useru a checbox je oznacen, dodjejujemo rolu
                                await UserManager.AddToRoleAsync(model.SingleUser.Id, role.Role.Name);
                            if (alreadyAsignedRole == role.Role.Name && role.IsChecked == false) // ako je rola pridruzena, a checbox nije oznacen, oduzimamo rolu
                                await UserManager.RemoveFromRoleAsync(model.SingleUser.Id, role.Role.Name);
                        }
                    }
                }               
            }
           else
            {
                View("Error");
            }
            return RedirectToAction("AddRoleToUser");
        }
    }
}