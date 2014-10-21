using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace pickuphockey.Controllers
{
    public class UsersController : Controller
    {
        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        public ActionResult Index()
        {
            return View(UserManager.Users.ToList().OrderBy(u => u.LastName));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult ToggleActive(string id, bool active)
        {
            var user = UserManager.FindById(id);
            if (user == null)
                return Json(new { Success = false, Message = "User not found" });

            user.Active = active;
            UserManager.Update(user);

            return Json(new { Success = true, Message = "Updated" });
        }
    }
}