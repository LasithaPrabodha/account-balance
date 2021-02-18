using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Transaction
    {
        [Column("TransactionId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Added Date Time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Timestamp]
        public DateTime AddedDateTime { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public int Year { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public int Month { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}