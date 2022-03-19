using APP.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace APP.Controllers
{
    [Services.Authorize(Roles = "Global administrator")]
    public class RoleController : Controller
    {
        private ApplicationRoleManager _roleManager;
         
        public RoleController()
        {
        }

        public RoleController(ApplicationRoleManager roleManager)
        {
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

        // show all roles on role index page
        public ActionResult Index()
        {
            List<RoleViewModel> list = new List<RoleViewModel>();
            foreach (var role in RoleManager.Roles)
                list.Add(new RoleViewModel(role));
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        // create a new role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                var role = new ApplicationRole() { Name = model.Name };
                var roleCreated = await RoleManager.CreateAsync(role);
                if(!roleCreated.Succeeded)
                {
                    return RedirectToAction("RoleError", "Error");
                }
                return RedirectToAction("Index");
            }

            return RedirectToAction("RoleError", "Error");
        }

       
        public async Task<ActionResult> Edit (string id)
        {
            if(id == null)
            {
                return RedirectToAction("RoleError", "Error");
            }
            var role = await RoleManager.FindByIdAsync(id);
            if(role == null)
            {
                return RedirectToAction("RoleError", "Error");
            }
            return View(new RoleViewModel(role));
        }

        // edit a role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                if (TempData["roleBeforeUpdate"] != null)
                {
                    string roleBeforeUpdate = (string)TempData["roleBeforeUpdate"];
                    if (roleBeforeUpdate != model.Name)
                    {
                        var role = new ApplicationRole() { Id = model.Id, Name = model.Name };
                        var updatedRole = await RoleManager.UpdateAsync(role);
                        if(!updatedRole.Succeeded)
                        {
                            return RedirectToAction("RoleError", "Error");
                        }
                    }
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("RoleError", "Error");
        }

        // see role details
        public async Task<ActionResult> Details(string id)
        {
            if(id == null)
            {
                return RedirectToAction("RoleError", "Error");
            }
            var role = await RoleManager.FindByIdAsync(id);
            if(role == null)
            {
                return RedirectToAction("RoleError", "Error");
            }
            return View(new RoleViewModel(role));
        }


        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("RoleError", "Error");
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToAction("RoleError", "Error");
            }
            return View(new RoleViewModel(role));
        }

        // delete a role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(RoleViewModel model)
        {
            if(model != null)
            {
                var role = await RoleManager.FindByIdAsync(model.Id); // gets a role by id
                if(role == null)
                {
                    return RedirectToAction("RoleError", "Error");
                }
                var deletedRole = await RoleManager.DeleteAsync(role);  // deletes the role
                if(!deletedRole.Succeeded)
                {
                    return RedirectToAction("RoleError", "Error");
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("RoleError", "Error");
        }
    }
}