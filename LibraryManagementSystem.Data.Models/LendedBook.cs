﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.LendedBook;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;

namespace LibraryManagementSystem.Data.Models
{
    /// <summary>
    /// Model representing lended books by the application user and other information used in the database.
    /// </summary>

    public class LendedBook
    {
        public LendedBook()
        {
            this.Id = Guid.NewGuid();
        }

        [Comment("Primary key")]
        [Key]
        public Guid Id { get; set; }

        [Comment("The date when the book was borrowed")]
        [Required]
        [DisplayFormat(DataFormatString = DateFormatt)]
        public DateTime LoanDate { get; set; }

        [Comment("The date when the book was returned")]
        [DisplayFormat(DataFormatString = GlobalDateFormat)]
        public DateTime? ReturnDate { get; set; }

        [Comment("BookId")]
        [Required]
        [ForeignKey(nameof(Book))]
        public Guid BookId { get; set; }

        [Comment("Book")]
        public virtual Book Book { get; set; } = null!;

        [Comment("Application User Id")]
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [Comment("User")]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
