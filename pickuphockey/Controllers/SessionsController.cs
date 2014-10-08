﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using pickuphockey.Models;
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
            session.ActivityLogs.ForEach(t => { t.User = UserManager.FindById(t.UserId); });

            return View(session);
        }

        // GET: Sessions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SessionId,SessionDate,Note")] Session session)
        {
            if (!ModelState.IsValid) return View(session);
            
            session.CreateDateTime = DateTime.UtcNow;
            session.UpdateDateTime = DateTime.UtcNow;
            var newSession = _db.Sessions.Add(session);
            _db.SaveChanges();

            AddActivity(newSession.SessionId, "Created Session");
            
            return RedirectToAction("Index");
        }

        // GET: Sessions/Edit/5
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
        public ActionResult Edit([Bind(Include = "SessionId,SessionDate,CreateDateTime,Note")] Session session)
        {
            if (!ModelState.IsValid) return View(session);

            AddActivity(session.SessionId, "Edited Session");

            session.UpdateDateTime = DateTime.UtcNow;
            _db.Entry(session).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Sessions/Delete/5
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
        public ActionResult DeleteConfirmed(int id)
        {
            var session = _db.Sessions.Find(id);
            _db.Sessions.Remove(session);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void AddActivity(int sessionId, string activity)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var activitylog = _db.ActivityLogs.Create();

            activitylog.UserId = user.Id;
            activitylog.SessionId = sessionId;
            activitylog.Activity = activity;
            activitylog.CreateDateTime = DateTime.UtcNow;

            _db.Entry(activitylog).State = EntityState.Added;
            _db.SaveChanges();
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
