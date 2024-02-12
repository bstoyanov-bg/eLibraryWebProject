using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Data.Models
{
    /// <summary>
    /// Model representing lended books by the application user and other information used in the database.
    /// Many to Many Table.
    /// </summary>

    public class LendedBook
    {
        [Comment("Primary key")]
        [Key]
        public Guid Id { get; set; }

        [Comment("The date when the book was borrowed")]
        [Required]
        public DateOnly LoanDate { get; set; }

        [Comment("The date when the book was returned")]
        public DateOnly? ReturnDate { get; set; }

        [Comment("BookId")]
        [Required]
        [ForeignKey(nameof(Book))]
        public Guid BookId { get; set; }

        [Comment("Book")]
        public Book Book { get; set; } = null!;

        [Comment("ApplicationUserId")]
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [Comment("Book")]
        public ApplicationUser User { get; set; } = null!;
    }
}
