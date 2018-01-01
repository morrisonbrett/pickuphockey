using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pickuphockey.Models
{
    public enum SessionAction
    {
        Buy,
        Sell
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
            _pstZone = TimeZoneInfo.FindSystemTimeZoneById(System.Configuration.ConfigurationManager.AppSettings["DisplayTimeZone"]);
            _sessionEndTime = TimeSpan.Parse(System.Configuration.ConfigurationManager.AppSettings["SessionEndTime"]);
            _buyDayMinimum = int.Parse(System.Configuration.ConfigurationManager.AppSettings["BuyDayMinimum"]);
			_defaultStartTime = TimeSpan.Parse(System.Configuration.ConfigurationManager.AppSettings["DefaultStartTime"]);
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
        public bool IsPast => SessionDate < TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _pstZone);

        [NotMapped]
        public bool CanDelete => SessionDate > TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _pstZone);

        [NotMapped]
        public bool CanEdit => SessionDate > TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _pstZone);

        [NotMapped]
        public bool CanBuy => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _pstZone) >= SessionDate.AddDays(-1 * _buyDayMinimum);

        [NotMapped]
        public DateTime BuyDateTime => SessionDate.AddDays(-1 * _buyDayMinimum);
    }
}