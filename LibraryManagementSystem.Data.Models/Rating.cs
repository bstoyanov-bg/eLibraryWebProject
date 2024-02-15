using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Rating;

namespace LibraryManagementSystem.Data.Models
{
    /// <summary>
    /// Model representing rating of book used in the database.
    /// </summary>

    public class Rating
    {
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

        [Comment("Rating for the book")]
        [Required]
        [MaxLength(BookRatingMaxLength)]
        public decimal BookRating { get; set; }

        [Comment("Comment for the book")]
        [Required]
        public string Comment { get; set; } = null!;

        [Comment("Date of the rating")]
        [Required]
        public DateOnly Date { get; set; }
    }
}
