using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace pickuphockey.Models
{
    public class ActivityLog
    {
        public int SessionId { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int ActivityLogId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string Activity { get; set; }

        public virtual Session Session { get; set; }
    }
}
