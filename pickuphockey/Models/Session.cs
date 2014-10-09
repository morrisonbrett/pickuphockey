using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace pickuphockey.Models
{
    public class Session
    {
        public Session()
        {
            ActivityLogs = new HashSet<ActivityLog>();
        }

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
    }
}