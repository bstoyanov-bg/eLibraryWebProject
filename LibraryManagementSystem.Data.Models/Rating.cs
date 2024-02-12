﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Rating;

namespace LibraryManagementSystem.Data.Models
{
    public class Rating
    {
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
