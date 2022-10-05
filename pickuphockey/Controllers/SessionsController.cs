using System;
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

            session.RegularSet = _db.RegularSets.Where(q => q.RegularSetId == session.RegularSetId).FirstOrDefault();
            session.Regulars = _db.Regulars.Where(q => q.RegularSetId == session.RegularSetId).ToList();
            session.Regulars.ForEach(t =>
            {
                t.User = UserManager.FindById(t.UserId);
                var buySell = _db.BuySell.Where(q => q.SessionId == id && !string.IsNullOrEmpty(q.SellerUserId) && q.SellerUserId == t.UserId).FirstOrDefault();
                t.SellingOrSoldSpot = buySell != null;
                if (t.TeamAssignment == TeamAssignment.Light && !t.SellingOrSoldSpot)
                {
                    session.LightCount++;
                }
                if (t.TeamAssignment == TeamAssignment.Dark && !t.SellingOrSoldSpot)
                {
                    session.DarkCount++;
                }
            });

            session.BuySells = _db.BuySell.Where(q => q.SessionId == session.SessionId).ToList();
            session.BuySells.ForEach(t =>
            {
                t.SellerUser = UserManager.FindById(t.SellerUserId);
                t.BuyerUser = UserManager.FindById(t.BuyerUserId);
            });

            session.LightSubs = session.BuySells.Where(r => r.BuyerUserId != null && r.TeamAssignment == TeamAssignment.Light).OrderBy(r => r.BuySellId).ToList();
            session.LightCount += session.LightSubs.Count();

            session.DarkSubs = session.BuySells.Where(r => r.BuyerUserId != null && r.TeamAssignment == TeamAssignment.Dark).OrderBy(r => r.BuySellId).ToList();
            session.DarkCount += session.DarkSubs.Count();

            // Go through entire buy sell list and find anyone that bought, and then sold
            session.BuySells.ForEach(t =>
            {
                var buySell = session.BuySells.Where(r => r.SellerUserId != null && !string.IsNullOrEmpty(r.SellerUserId) && r.SellerUserId == t.BuyerUserId).FirstOrDefault();
                if (buySell != null)
                {
                    t.ReSellingOrSold = true;
                    if (t.TeamAssignment == TeamAssignment.Light)
                    {
                        session.LightCount--;
                    }
                    else if (t.TeamAssignment == TeamAssignment.Dark)
                    {
                        session.DarkCount--;
                    }
                }
            });

            var userid = Thread.CurrentPrincipal.Identity.GetUserId();
            session.User = UserManager.FindById(userid);

            return View(session);
        }

        // GET: Sessions/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            // TODO change order by day of week
            ViewBag.RegularSets = _db.RegularSets.OrderByDescending(t => t.CreateDateTime).ToList();
            ViewBag.AllRegulars = _db.Regulars.ToList();

            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "SessionId,SessionDate,Note,RegularSetId")] Session session)
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
            if (Request.Url == null) return;

            var userid = Thread.CurrentPrincipal.Identity.GetUserId();

            var user = UserManager.FindById(userid);

            var sessionurl = Url.Action("Details", "Sessions", new { id = session.SessionId }, Request.Url.Scheme);

            var subject = "Session " + session.SessionDate.ToString("dddd, MM/dd/yyyy, HH:mm") + " CREATED";
            var body = "A new pickup session has been created by " + user.FirstName + " " + user.LastName + " for " + session.SessionDate.ToString("dddd, MM/dd/yyyy, HH:mm") + "." + Environment.NewLine + Environment.NewLine;
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

            // TODO change order by day of week
            ViewBag.RegularSets = _db.RegularSets.OrderByDescending(t => t.CreateDateTime).ToList();
            ViewBag.AllRegulars = _db.Regulars.ToList();

            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "SessionId,SessionDate,CreateDateTime,Note,RegularSetId")] Session session)
        {
            if (!ModelState.IsValid) return View(session);

            var fsession = _db.Sessions.AsNoTracking().FirstOrDefault(t => t.SessionId == session.SessionId);
            if (fsession == null || !fsession.CanEdit)
            {
                return HttpNotFound();
            }

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
            if (session == null || !session.CanDelete)
            {
                return HttpNotFound();
            }

            _db.Sessions.Remove(session);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Sessions/RegularSet/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult RegularSet(int? id)
        {
            if (id == null)
            {
                return Json(new { Success = false, Message = "Invalid Request" });
            }

            var regulars = _db.Regulars.Where(r => r.RegularSetId == id).ToList();
            if (regulars == null)
            {
                return Json(new { Success = false, Message = "Not Found" });
            }

            var rs = _db.RegularSets.Find(id);
            regulars.ForEach(r =>
            {
                r.RegularSet = rs;
                r.User = UserManager.FindById(r.UserId);
            });

            regulars = regulars.OrderByDescending(r => r.PositionPreference).ThenBy(u => u.User.FirstName).ToList();

            return Json(regulars);
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
