using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace pickuphockey.Models
{
    public enum SessionAction
    {
        Buy,
        Sell
    }

    public enum LockerRoom13Status
    {
        In,
        Maybe,
        Out
    }

    public class LockerRoom13User
    {
        public LockerRoom13Status lockerRoom13Status { get; set; }
        public ApplicationUser user { get; set; }
    }

    public class Session
    {
        private readonly TimeZoneInfo _pstZone;
        private readonly TimeSpan _sessionEndTime;
        private readonly int _buyDayMinimum;
		public readonly TimeSpan _defaultStartTime;

        public Session()
        {
            ActivityLogs = new HashSet<ActivityLog>();
            BuySells = new HashSet<BuySell>();
            LockerRoom13Users = new HashSet<LockerRoom13User>();
            _pstZone = TimeZoneInfo.FindSystemTimeZoneById(System.Configuration.ConfigurationManager.AppSettings["DisplayTimeZone"]);
            _sessionEndTime = TimeSpan.Parse(System.Configuration.ConfigurationManager.AppSettings["SessionEndTime"]);
            _buyDayMinimum = int.Parse(System.Configuration.ConfigurationManager.AppSettings["BuyDayMinimum"]);
			_defaultStartTime = TimeSpan.Parse(System.Configuration.ConfigurationManager.AppSettings["DefaultStartTime"]);
		}

        public bool CanBuy(bool IsPreferred, bool IsPreferredPlus, bool IsAdmin)
        {
            if (IsAdmin)
                return true;

            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _pstZone);
            var timeCanBuy = SessionDate.AddDays(-1 * (BuyDayMinimum != null ? (int) BuyDayMinimum : _buyDayMinimum)).AddHours(2);

            return timeNow >= (IsPreferredPlus ? timeCanBuy.AddDays(-1).AddMinutes(-5) : IsPreferred ? timeCanBuy.AddDays(-1) : timeCanBuy);
        }

        public int HasRosterSpot()
        {
            if (RegularSetId == null || Regulars == null || Regulars.Count == 0)
                return -1;

            // See if the User has a roster regular spot for this session
            var r = Regulars.Where(t => t.UserId == User.Id);
            if (r.Any())
                return 1;

            return 0;
        }

        public bool AlreadyBuying()
        {
            var bs = BuySells.Where(q => !string.IsNullOrEmpty(q.BuyerUserId) && q.BuyerUserId == User.Id && string.IsNullOrEmpty(q.SellerUserId));

            return bs.Any();
        }

        public bool AlreadyBought()
        {
            var bs = BuySells.Where(q => !string.IsNullOrEmpty(q.BuyerUserId) && q.BuyerUserId == User.Id && !string.IsNullOrEmpty(q.SellerUserId));

            return bs.Any();
        }

        public bool AlreadySelling()
        {
            var bs = BuySells.Where(q => !string.IsNullOrEmpty(q.SellerUserId) && q.SellerUserId == User.Id && string.IsNullOrEmpty(q.BuyerUserId));

            return bs.Any();
        }

        public bool AlreadySold()
        {
            var bs = BuySells.Where(q => !string.IsNullOrEmpty(q.SellerUserId) && q.SellerUserId == User.Id && !string.IsNullOrEmpty(q.BuyerUserId));

            return bs.Any();
        }

        [Key]
        public int SessionId { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, MM/dd/yyyy, HH:mm}")]
		[DisplayName("Session Date")]
        public DateTime SessionDate { get; set; }

        [DisplayName("Created")]
        public DateTime CreateDateTime { get; set; }
        
        [DisplayName("Updated")]
        public DateTime UpdateDateTime { get; set; }

        [DisplayName("Note")]
        public string Note { get; set; }

        [DisplayName("Activity")]
        public ICollection<ActivityLog> ActivityLogs { get; set; }

        [DisplayName("Buyers and Sellers")]
        public ICollection<BuySell> BuySells { get; set; }

        [NotMapped]
        public ApplicationUser User;

        [NotMapped]
        public bool IsPast => SessionDate < TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _pstZone);

        [NotMapped]
        public bool CanDelete => SessionDate > TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _pstZone);

        [NotMapped]
        public bool CanEdit => SessionDate > TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _pstZone);

        [NotMapped]
        public DateTime BuyDateTime => SessionDate.AddDays(-1 * (BuyDayMinimum != null ? (int) BuyDayMinimum : _buyDayMinimum)).AddHours(2);

        [DisplayName("Roster Set")]
        public int ?RegularSetId { get; set; }

        [ForeignKey("RegularSetId")]
        public virtual RegularSet RegularSet { get; set; }

        [DisplayName("Roster")]
        [NotMapped]
        public virtual ICollection<Regular> Regulars { get; set; }

        [DisplayName("LockerRoom13Users")]
        [NotMapped]
        public virtual ICollection<LockerRoom13User> LockerRoom13Users { get; set; }

        [NotMapped]
        public ICollection<BuySell> LightSubs { get; set; }

        [NotMapped]
        public ICollection<BuySell> DarkSubs { get; set; }

        [NotMapped]
        public int LightCount { get; set; }

        [NotMapped]
        public int DarkCount { get; set; }

        [NotMapped]
        public decimal LightTotalRating { get; set; }

        [NotMapped]
        public decimal DarkTotalRating { get; set; }

        [NotMapped]
        public ICollection<BuySell> UnmarkedReceived { get; set; }

        [NotMapped]
        public ICollection<BuySell> UnmarkedSent { get; set; }

        [DisplayName("Buy Day Minimum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        public int ?BuyDayMinimum { get; set; }
    }
}