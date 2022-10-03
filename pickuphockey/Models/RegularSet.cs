using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace pickuphockey.Models
{
    [Table("RegularSet")]
    public class RegularSet
    {
        [Key]
        public int RegularSetId { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Day Of Week")]
        public int DayOfWeek { get; set; }

        [DisplayName("Created")]
        public DateTime CreateDateTime { get; set; }
    }

    [Table("Regulars")]
    public class Regular
    {
        [Required]
        [ForeignKey("RegularSet")]
        public int RegularSetId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public string UserId { get; set; }

        [DisplayName("Team Assignment")]
        public TeamAssignment TeamAssignment { get; set; }

        [DisplayName("Position Preference")]
        public PositionPreference PositionPreference { get; set; }
    }
}