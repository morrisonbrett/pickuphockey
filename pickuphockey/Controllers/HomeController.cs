using System;
using System.Collections.Generic;
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

        public ActionResult LockerRoom13()
        {
            var pstZone = TimeZoneInfo.FindSystemTimeZoneById(System.Configuration.ConfigurationManager.AppSettings["DisplayTimeZone"]);
            var nowTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pstZone);

            var sessions = _db.Sessions.Where(m => m.SessionDate > nowTime && !(!string.IsNullOrEmpty(m.Note) && m.Note.Contains("CANCELLED"))).OrderBy(m => m.SessionDate).ToList();
            foreach (var s in sessions)
            {
                // Get the user list of all active LR13 members
                var users = _db.Users.Where(u => u.LockerRoom13 == true).OrderBy(u => u.LastName).ToList();

                // Get the regulars for this session
                s.RegularSet = _db.RegularSets.Where(q => q.RegularSetId == s.RegularSetId).FirstOrDefault();
                s.Regulars = _db.Regulars.Where(q => q.RegularSetId == s.RegularSetId).ToList();
                s.BuySells = _db.BuySell.Where(q => q.SessionId == s.SessionId).ToList();

                var lockerRoom13Users = new List<LockerRoom13User>();

                // For each user in that list, determine their status for this session
                foreach (var user in users) {
                    var lockerRoom13User = new LockerRoom13User();

                    lockerRoom13User.user = user;

                    // See if the user is in this roster
                    var reg = s.Regulars.Where(u => u.UserId == user.Id).FirstOrDefault();
                    if (reg != null)
                    {
                        lockerRoom13User.lockerRoom13Status = LockerRoom13Status.In;
                    }
                    else
                    {
                        lockerRoom13User.lockerRoom13Status = LockerRoom13Status.Out;
                    }

                    var sellingOrSold = s.BuySells.Where(bs => bs.SellerUserId == user.Id).FirstOrDefault();
                    var buyingOrBought = s.BuySells.Where(bs => bs.BuyerUserId == user.Id).FirstOrDefault();
                    var buyingButNotBought = s.BuySells.Where(bs => bs.BuyerUserId == user.Id && string.IsNullOrEmpty(bs.SellerUserId)).FirstOrDefault();
                    var buyingAndGotSpot = s.BuySells.Where(bs => bs.BuyerUserId == user.Id && !string.IsNullOrEmpty(bs.SellerUserId)).FirstOrDefault();

                    // See if the user is selling or sold
                    if (sellingOrSold != null)
                    {
                        lockerRoom13User.lockerRoom13Status = LockerRoom13Status.Out;

                        // So they're selling, and they're out. But now check if maybe they're buying after they sold
                        if (buyingOrBought != null && buyingOrBought.BuySellId > sellingOrSold.BuySellId)
                        {
                            if (buyingAndGotSpot != null)
                            {
                                lockerRoom13User.lockerRoom13Status = LockerRoom13Status.In;
                            }
                            else if (buyingButNotBought != null)
                            {
                                lockerRoom13User.lockerRoom13Status = LockerRoom13Status.Maybe;
                            }
                        }
                    }
                    else // They're not selling and haven't sold. But maybe they're just buying
                    {
                        // See if the user is buying but doesn't have a spot yet
                        if (buyingButNotBought != null)
                        {
                            lockerRoom13User.lockerRoom13Status = LockerRoom13Status.Maybe;
                        }

                        // See if the user bought and got a spot
                        if (buyingAndGotSpot != null)
                        {
                            lockerRoom13User.lockerRoom13Status = LockerRoom13Status.In;
                        }
                    }

                    lockerRoom13Users.Add(lockerRoom13User);
                }

                s.LockerRoom13Users = lockerRoom13Users;
            }

            return View(sessions);
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