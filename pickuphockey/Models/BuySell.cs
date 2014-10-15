using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pickuphockey.Models
{
    public class BuySell
    {
        [Required]
        public int SessionId { get; set; }

        [Key]
        public int BuySellId { get; set; }

        public string BuyerUserId { get; set; }

        [ForeignKey("BuyerUserId")]
        public ApplicationUser BuyerUser { get; set; }

        public string SellerUserId { get; set; }

        [ForeignKey("SellerUserId")]
        public ApplicationUser SellerUser { get; set; }

        [DisplayName("Seller Note")]
        public string SellerNote { get; set; }

        [DisplayName("Payment Preference")]
        public PaymentPreference PaymentPreference { get; set; }

        [DisplayName("Payment Info")]
        public string PaymentInfo { get; set; }

        [DisplayName("Team Assignment")]
        public TeamAssignment TeamAssignment { get; set; }

        [DisplayName("Buyer Note")]
        public string BuyerNote { get; set; }

        [DisplayName("Payment Sent")]
        public bool PaymentSent { get; set; }

        [DisplayName("Payment Received")]
        public bool PaymentReceived { get; set; }

        [DisplayName("Created")]
        public DateTime CreateDateTime { get; set; }

        [DisplayName("Updated")]
        public DateTime UpdateDateTime { get; set; }

        [DisplayName("Session")]
        [ForeignKey("SessionId")]
        public virtual Session Session { get; set; }
    }
}
