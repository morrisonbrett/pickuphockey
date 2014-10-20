using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using pickuphockey.Models;
using pickuphockey.Services;
using WebGrease.Css.Extensions;

namespace pickuphockey.Controllers
{
    public class SessionsController : Controller
    {
        public SessionsController()
        {
        }

        public SessionsController(ApplicationUserManager userManager)
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

        // GET: Sessions
        public ActionResult Index()
        {
            return View(_db.Sessions.ToList().OrderByDescending(t => t.SessionDate));
        }

        // GET: Sessions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var session = _db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }

            // TODO Optimize this query into one statement
            session.ActivityLogs = _db.ActivityLogs.Where(q => q.SessionId == session.SessionId).ToList();
            session.ActivityLogs.ForEach(t =>
            {
                t.User = UserManager.FindById(t.UserId);
            });

            session.BuySells = _db.BuySell.Where(q => q.SessionId == session.SessionId).ToList();
            session.BuySells.ForEach(t =>
            {
                t.SellerUser = UserManager.FindById(t.SellerUserId);
                t.BuyerUser = UserManager.FindById(t.BuyerUserId);
            });

            return View(session);
        }

        // GET: Sessions/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "SessionId,SessionDate,Note")] Session session)
        {
            if (!ModelState.IsValid) return View(session);
            
            session.CreateDateTime = DateTime.UtcNow;
            session.UpdateDateTime = DateTime.UtcNow;
            var newSession = _db.Sessions.Add(session);
            _db.SaveChanges();

            _db.AddActivity(newSession.SessionId, "Created Session");

            EmailSession(newSession);
            
            return RedirectToAction("Index");
        }

        private void EmailSession(Session session)
        {
            var userid = Thread.CurrentPrincipal.Identity.GetUserId();

            var user = UserManager.FindById(userid);

            var sessionurl = Url.Action("Details", "Sessions", new { id = session.SessionId }, protocol: Request.Url.Scheme);

            var subject = "Session " + session.SessionDate.ToString("dddd, MM/dd/yyyy") + " CREATED";
            var body = "A new pickup session has been created by " + user.FirstName + " " + user.LastName + " for " + session.SessionDate.ToString("dddd, MM/dd/yyyy") + "." + Environment.NewLine + Environment.NewLine;
            if (!string.IsNullOrEmpty(session.Note))
            {
                body += session.Note + Environment.NewLine + Environment.NewLine;
            }
            body += "Click here for the details: " + sessionurl + Environment.NewLine;

            var emailServices = new EmailServices();
            var users = UserManager.Users.ToList().Where(t => t.Active && t.NotificationPreference != NotificationPreference.None);
            foreach (var u in users)
            {
                emailServices.SendMail(subject, body, u.Email);
            }
        }

        // GET: Sessions/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var session = _db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "SessionId,SessionDate,CreateDateTime,Note")] Session session)
        {
            if (!ModelState.IsValid) return View(session);

            _db.AddActivity(session.SessionId, "Edited Session");

            session.UpdateDateTime = DateTime.UtcNow;
            _db.Entry(session).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Sessions/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var session = _db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var session = _db.Sessions.Find(id);
            _db.Sessions.Remove(session);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
