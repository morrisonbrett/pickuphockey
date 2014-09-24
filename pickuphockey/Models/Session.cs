using System;
using System.ComponentModel.DataAnnotations;

namespace pickuphockey.Models
{
    public class Session
    {
        public int SessionId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, MM/dd/yyyy}")]
        public DateTime SessionDate { get; set; }

        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}