using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using pickuphockey.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

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
            return View(UserManager.Users.ToList().OrderBy(u => u.LastName));
        }

        public ActionResult Index()
        {
            return View(UserManager.Users.ToList().OrderBy(u => u.LastName));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult TogglePreferred(string id, bool preferred)
        {
            var user = UserManager.FindById(id);
            if (user == null)
                return Json(new { Success = false, Message = "User not found" });

            user.Preferred = preferred;
            UserManager.Update(user);

            return Json(new { Success = true, Message = "Updated Preferred" });
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
        public ActionResult Details(string id)
        {
            var user = UserManager.FindById(id);
            if (user == null)
                return Json(new { Success = false, Message = "User not found" });

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Details([Bind(Include = "Id,FirstName,LastName,UserName,Email,PayPalEmail,VenmoAccount,MobileLast4,NotificationPreference,Active,Preferred,Rating")] ApplicationUser user)
        {
            if (!ModelState.IsValid) return View(user);

            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();

            return View(user);
        }

    }
}