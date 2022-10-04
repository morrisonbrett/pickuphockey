using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace pickuphockey.Models
{
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

    public class Regular
    {
        [Key, Column("RegularSetId", Order=0)]
        public int RegularSetId { get; set; }

        [Key, Column("UserId", Order=1)]
        public string UserId { get; set; }

        [ForeignKey("RegularSetId")]
        [NotMapped]
        public RegularSet RegularSet { get; set; }

        [ForeignKey("UserId")]
        [NotMapped]
        public ApplicationUser User;

        [DisplayName("Team Assignment")]
        public TeamAssignment TeamAssignment { get; set; }

        [DisplayName("Position Preference")]
        public PositionPreference PositionPreference { get; set; }
    }
}