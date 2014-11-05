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
        public Session()
        {
            ActivityLogs = new HashSet<ActivityLog>();
            BuySells = new HashSet<BuySell>();
            _pstZone = TimeZoneInfo.FindSystemTimeZoneById(System.Configuration.ConfigurationManager.AppSettings["DisplayTimeZone"]);
        }

        [Key]
        public int SessionId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, MM/dd/yyyy}")]
        [DisplayName("Session")]
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
        public bool IsPast
        {
            get
            {
                return SessionDate.Date < TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _pstZone).Date;
            }
        }

        [NotMapped]
        public bool CanDelete
        {
            get
            {
                return SessionDate.Date >= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _pstZone).Date;
            }
        }

        [NotMapped]
        public bool CanEdit
        {
            get
            {
                return SessionDate.Date >= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _pstZone).Date;
            }
        }
    }
}