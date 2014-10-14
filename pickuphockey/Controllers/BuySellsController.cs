using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using pickuphockey.Services;
using pickuphockey.Models;

namespace pickuphockey.Controllers
{
    public class BuySellsController : Controller
    {
        private readonly EmailServices _emailServices;

        public BuySellsController()
        {
            _emailServices = new EmailServices();
        }

        public BuySellsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
            _emailServices = new EmailServices();
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

        private void SendSessionEmail(Session session, ApplicationUser seller, ApplicationUser buyer, SessionAction sessionAction)
        {
            var sessionurl = Url.Action("Details", "Sessions", new { id = session.SessionId }, protocol: Request.Url.Scheme);

            switch (sessionAction)
            {
                case SessionAction.Buy:
                case SessionAction.Sell:
                {
                    var subject = "Session " + session.SessionDate.ToString("dddd, MM/dd/yyyy") + " SOLD";
                    var body = "Your spot has been sold to " + buyer.FirstName + " " + buyer.LastName + "." + Environment.NewLine + Environment.NewLine;
                    body += "Click here for the details: " + sessionurl + Environment.NewLine;
                    if (seller.NotificationPreference != NotificationPreference.None)
                        _emailServices.SendMail(subject, body, seller.Email);

                    subject = "Session " + session.SessionDate.ToString("dddd, MM/dd/yyyy") + " BOUGHT";
                    body = "You bought a spot from " + seller.FirstName + " " + seller.LastName + "." + Environment.NewLine + Environment.NewLine;
                    body += "Click here for the details: " + sessionurl + Environment.NewLine;
                    if (seller.NotificationPreference != NotificationPreference.None)
                        _emailServices.SendMail(subject, body, buyer.Email);

                    break;
                }

                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        // GET: BuySells/Buy/5
        public ActionResult Buy(int? id)
        {
            var session = _db.Sessions.Find(id);
            if (id == null || session == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pstZone = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["DisplayTimeZone"]);
            if (session.SessionDate < TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pstZone).Date)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.SessionDate = session.SessionDate;
            var buyer = UserManager.FindById(User.Identity.GetUserId());

            // Look for open sell spot, if none, just create empty model
            var buySell = _db.BuySell.Where(q => q.SessionId == id && !string.IsNullOrEmpty(q.SellerUserId) && string.IsNullOrEmpty(q.BuyerUserId)).OrderBy(d => d.CreateDateTime).FirstOrDefault();
            if (buySell == null)
            {
                buySell = new BuySell { SessionId = id.Value };
            }
            else
            {
                buySell.SellerUser = UserManager.FindById(buySell.SellerUserId);
            }

            buySell.BuyerUser = buyer;
            buySell.BuyerUserId = buyer.Id;

            if (buySell.SellerUserId == buyer.Id)
            {
                ModelState.AddModelError("", "You cannot buy from yourself");
            }

            return View(buySell);
        }

        // POST: BuySells/Buy
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Buy([Bind(Include = "BuySellId,SessionId,BuyerNote,SellerUserId")] BuySell buySell)
        {
            var session = _db.Sessions.Find(buySell.SessionId);
            if (session == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pstZone = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["DisplayTimeZone"]);
            if (session.SessionDate < TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pstZone).Date)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var buyer = UserManager.FindById(User.Identity.GetUserId());
            var sessionurl = Url.Action("Details", "Sessions", new { id = session.SessionId }, protocol: Request.Url.Scheme);

            // Can't buy from self
            if (buySell.SellerUserId == buyer.Id)
            {
                return RedirectToAction("Details", "Sessions", new { id = buySell.SessionId });
            }

            if (ModelState.IsValid)
            {
                string activity;
                IEnumerable<ApplicationUser> users;

                if (!string.IsNullOrEmpty(buySell.SellerUserId))
                {
                    var updateBuySell = _db.BuySell.First(q => q.BuySellId == buySell.BuySellId);

                    var seller = UserManager.FindById(updateBuySell.SellerUserId);

                    activity = buyer.FirstName + " " + buyer.LastName + " BOUGHT SPOT FROM " + seller.FirstName + " " + seller.LastName;

                    updateBuySell.BuyerUserId = buyer.Id;
                    updateBuySell.BuyerNote = buySell.BuyerNote;
                    updateBuySell.UpdateDateTime = DateTime.UtcNow;
                    _db.Entry(updateBuySell).State = EntityState.Modified;

                    SendSessionEmail(session, seller, buyer, SessionAction.Buy);
                    users = UserManager.Users.ToList().Where(t => t.NotificationPreference == NotificationPreference.All && t.Id != seller.Id && t.Id != buyer.Id);
                }
                else
                {
                    activity = buyer.FirstName + " " + buyer.LastName + " added to BUYING list";

                    buySell.BuyerUserId = User.Identity.GetUserId();
                    buySell.BuyerNote = buySell.BuyerNote;
                    buySell.UpdateDateTime = DateTime.UtcNow;

                    buySell.CreateDateTime = DateTime.UtcNow;

                    _db.BuySell.Add(buySell);

                    users = UserManager.Users.ToList().Where(t => t.NotificationPreference == NotificationPreference.All && t.Id != buyer.Id);
                }

                _db.SaveChanges();

                _db.AddActivity(buySell.SessionId, activity);

                foreach (var u in users)
                {
                    var subject = "Session " + session.SessionDate.ToString("dddd, MM/dd/yyyy") + " ACTIVITY";
                    var body = activity + "." + Environment.NewLine + Environment.NewLine;
                    body += "Click here for the details: " + sessionurl + Environment.NewLine;

                    _emailServices.SendMail(subject, body, u.Email);
                }

                return RedirectToAction("Details", "Sessions", new { id = buySell.SessionId});
            }

            var newbuySell = new BuySell { SessionId = buySell.SessionId, BuyerUserId = User.Identity.GetUserId() };

            return View(newbuySell);
        }

        // GET: BuySells/Sell/5
        public ActionResult Sell(int? id)
        {
            var session = _db.Sessions.Find(id);
            if (id == null || session == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pstZone = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["DisplayTimeZone"]);
            if (session.SessionDate < TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pstZone).Date)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.SessionDate = session.SessionDate;
            var seller = UserManager.FindById(User.Identity.GetUserId());

            // Look for open buy spot, if none, just create empty model
            var buySell = _db.BuySell.Where(q => q.SessionId == id && string.IsNullOrEmpty(q.SellerUserId) && !string.IsNullOrEmpty(q.BuyerUserId)).OrderBy(d => d.CreateDateTime).FirstOrDefault();
            if (buySell == null)
            {
                buySell = new BuySell { SessionId = id.Value };
            }
            else
            {
                buySell.BuyerUser = UserManager.FindById(buySell.BuyerUserId);
            }

            buySell.SellerUser = seller;
            buySell.SellerUserId = seller.Id;
            buySell.PaymentPreference = seller.PaymentPreference;
            buySell.TeamAssignment = seller.TeamAssignment;

            // Can't sell to self
            if (buySell.BuyerUserId == seller.Id)
            {
                ModelState.AddModelError("", "You cannot sell to yourself");
            }

            // Look if self already has a spot that's unsold
            var openSell = _db.BuySell.Where(q => q.SessionId == id && q.SellerUserId == seller.Id && string.IsNullOrEmpty(q.BuyerUserId));
            if (openSell.Any())
            {
                ModelState.AddModelError("", "You already have a spot for sale");
            }

            return View(buySell);
        }

        // POST: BuySells/Buy
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sell([Bind(Include = "BuySellId,SessionId,SellerNote,PaymentPreference,TeamAssignment,BuyerUserId")] BuySell buySell)
        {
            var session = _db.Sessions.Find(buySell.SessionId);
            if (session == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pstZone = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["DisplayTimeZone"]);
            if (session.SessionDate < TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pstZone).Date)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var seller = UserManager.FindById(User.Identity.GetUserId());
            var sessionurl = Url.Action("Details", "Sessions", new { id = session.SessionId }, protocol: Request.Url.Scheme);

            // Can't sell to self
            if (buySell.BuyerUserId == seller.Id)
            {
                return RedirectToAction("Details", "Sessions", new { id = buySell.SessionId });
            }

            // Look if self already has a spot that's unsold
            var openSell = _db.BuySell.Where(q => q.SessionId == buySell.SessionId && q.SellerUserId == seller.Id && string.IsNullOrEmpty(q.BuyerUserId));
            if (openSell.Any())
            {
                return RedirectToAction("Details", "Sessions", new { id = buySell.SessionId });
            }

            if (ModelState.IsValid)
            {
                string activity;
                IEnumerable<ApplicationUser> users;

                if (!string.IsNullOrEmpty(buySell.BuyerUserId))
                {
                    var updateBuySell = _db.BuySell.First(q => q.BuySellId == buySell.BuySellId);

                    var buyer = UserManager.FindById(updateBuySell.BuyerUserId);

                    activity = seller.FirstName + " " + seller.LastName + " SOLD SPOT TO " + buyer.FirstName + " " + buyer.LastName;

                    updateBuySell.SellerUserId = seller.Id;
                    updateBuySell.SellerNote = buySell.SellerNote;
                    updateBuySell.PaymentPreference = buySell.PaymentPreference;
                    updateBuySell.TeamAssignment = buySell.TeamAssignment;
                    updateBuySell.UpdateDateTime = DateTime.UtcNow;

                    _db.Entry(updateBuySell).State = EntityState.Modified;

                    SendSessionEmail(session, seller, buyer, SessionAction.Sell);
                    users = UserManager.Users.ToList().Where(t => t.NotificationPreference == NotificationPreference.All && t.Id != seller.Id && t.Id != buyer.Id);
                }
                else
                {
                    activity = seller.FirstName + " " + seller.LastName + " added to SELLING list";

                    buySell.CreateDateTime = DateTime.UtcNow;
                    buySell.UpdateDateTime = DateTime.UtcNow;
                    buySell.SellerUserId = User.Identity.GetUserId();
                    buySell.SellerNote = buySell.SellerNote;

                    _db.BuySell.Add(buySell);

                    users = UserManager.Users.ToList().Where(t => t.NotificationPreference == NotificationPreference.All && t.Id != seller.Id);
                }

                _db.SaveChanges();

                _db.AddActivity(buySell.SessionId, activity);

                // Update the users preferences with the values from this sell.
                var user = UserManager.FindById(User.Identity.GetUserId());
                user.PaymentPreference = buySell.PaymentPreference;
                user.TeamAssignment = buySell.TeamAssignment;
                UserManager.Update(user);

                foreach (var u in users)
                {
                    var subject = "Session " + session.SessionDate.ToString("dddd, MM/dd/yyyy") + " ACTIVITY";
                    var body = activity + "." + Environment.NewLine + Environment.NewLine;
                    body += "Click here for the details: " + sessionurl + Environment.NewLine;

                    _emailServices.SendMail(subject, body, u.Email);
                }

                return RedirectToAction("Details", "Sessions", new { id = buySell.SessionId });
            }

            var newbuySell = new BuySell { SessionId = buySell.SessionId, SellerUserId = User.Identity.GetUserId() };

            return View(newbuySell);
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
