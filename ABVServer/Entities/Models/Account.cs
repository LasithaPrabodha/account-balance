
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{

    public class Account
    {
        [Column("AccountId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Account Name")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Timestamp]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }


    }
}