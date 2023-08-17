using System.Linq;
using System.Web.Mvc;
using pickuphockey.Models;

namespace pickuphockey.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index(ManageController.ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageController.ManageMessageId.CheckEmailVerification ? "Check your email to confirm your account"
                : "";

            return View(_db.Sessions.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = System.Configuration.ConfigurationManager.AppSettings["SiteTitle"];

            return View();
        }

        public ActionResult Privacy()
        {
            ViewBag.Message = System.Configuration.ConfigurationManager.AppSettings["SiteTitle"];

            return View();
        }

        public ActionResult Calendar()
        {
            ViewBag.Message = System.Configuration.ConfigurationManager.AppSettings["SiteTitle"];

            return View();
        }
    }
}