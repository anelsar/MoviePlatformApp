using APP.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace APP.Controllers
{
    [Authorize(Roles = "Admin")]
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
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                var role = new ApplicationRole() { Name = model.Name };
                await RoleManager.CreateAsync(role);
            }
            
            return RedirectToAction("Index");   
        }

       
        public async Task<ActionResult> Edit (string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }

        // edit a role
        [HttpPost]
        public async Task<ActionResult> Edit(RoleViewModel model)
        {
            if(TempData["roleBeforeUpdate"] != null)
            {
                string roleBeforeUpdate = (string)TempData["roleBeforeUpdate"];
                if(roleBeforeUpdate != model.Name)
                {
                    var role = new ApplicationRole() { Id = model.Id, Name = model.Name };
                    await RoleManager.UpdateAsync(role);
                }                
            }
            
            return RedirectToAction("Index");
        }

        // see role details
        public async Task<ActionResult> Details(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }


        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }

        // delete a role
        [HttpPost]
        public async Task<ActionResult> Delete(RoleViewModel model)
        {
            
            var role = await RoleManager.FindByIdAsync(model.Id); // gets a role by id
            await RoleManager.DeleteAsync(role);  // deletes the role
            return RedirectToAction("Index");
        }
    }
}