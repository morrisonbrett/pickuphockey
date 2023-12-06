using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using pickuphockey.Models;

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

        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult DownloadActive()
        {
            return View(UserManager.Users.Where(u => u.Active == true).ToList().OrderBy(u => u.LastName));
        }

        public ActionResult Index(bool showInactive = false)
        {
            IEnumerable<ApplicationUser> list;

            if (showInactive)
                list = UserManager.Users.ToList().OrderBy(u => u.LastName);
            else
                list = UserManager.Users.Where(u => u.Active == true).ToList().OrderBy(u => u.LastName);

            return View(list);
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

            return Json(new { Success = true, Message = "Updated Active" });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id, string message)
        {
            var user = UserManager.FindById(id);
            if (user == null)
                return Json(new { Success = false, Message = "User not found" });

            ViewBag.StatusMessage = message;
            
            return View(user);
        }

        public ActionResult GamePucks()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Details([Bind(Include = "Id,FirstName,LastName,UserName,Email,PayPalEmail,VenmoAccount,MobileLast4,NotificationPreference,Active,Preferred,PreferredPlus,Rating,EmergencyName,EmergencyPhone,LockerRoom13")] ApplicationUser user)
        {
            if (!ModelState.IsValid) return View(user);

            var u = UserManager.FindById(user.Id);
            if (u == null)
                return View(user);
            
            u.FirstName = user.FirstName;
            u.LastName = user.LastName;
            u.UserName = user.UserName;
            u.Email = user.Email;
            u.PayPalEmail = user.PayPalEmail;
            u.VenmoAccount = user.VenmoAccount;
            u.MobileLast4 = user.MobileLast4;
            u.NotificationPreference = user.NotificationPreference;
            u.Active = user.Active;
            u.Preferred = user.Preferred;
            u.PreferredPlus = user.PreferredPlus;
            u.Rating = user.Rating;
            u.EmergencyName = user.EmergencyName;
            u.EmergencyPhone = user.EmergencyPhone;
            u.LockerRoom13 = user.LockerRoom13;

            UserManager.Update(u);

            return RedirectToAction("Details", new { id = user.Id, Message = "User settings saved" });
        }
    }
}