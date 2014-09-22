using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using pickuphockey.Models;

namespace pickuphockey.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            return View(db.Sessions.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = System.Configuration.ConfigurationManager.AppSettings["SiteTitle"];

            return View();
        }
    }
}