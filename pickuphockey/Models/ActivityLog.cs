using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace pickuphockey.Models
{
    public class ActivityLog
    {
        public int SessionId { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int ActivityLogId { get; set; }

        [DisplayName("Created")]
        public DateTime CreateDateTime { get; set; }

        [DisplayName("Activity")]
        public string Activity { get; set; }

        [DisplayName("Session")]
        public virtual Session Session { get; set; }
    }
}
