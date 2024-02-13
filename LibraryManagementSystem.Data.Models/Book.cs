﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Book;

namespace LibraryManagementSystem.Data.Models
{
    /// <summary>
    /// Model representing book entity used in the database.
    /// </summary>
    
    public class Book
    {
        [Comment("Primary key")]
        [Key]
        public Guid Id { get; set; }

        [Comment("International Standard Book Number")]
        [RegularExpression(ISBNRegexPattern)]
        public string? ISBN { get; set; }

        [Comment("Title of the book")]
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Comment("The year of book publish")]
        [MaxLength(YearPublishedMaxLength)]
        public DateOnly? YearPublished { get; set; }

        [Comment("Description of the book")]
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Comment("Publisher of the book")]
        [MaxLength(PublisherMaxLength)]
        public string? Publisher { get; set; }

        [Comment("Cover image of the book")]
        [MaxLength(CoverImagePathUrlMaxLength)]
        public string? CoverImagePathUrl { get; set; }

        [Comment("AuthorId")]
        [Required]
        [ForeignKey(nameof(Author))]
        public Guid AuthorId { get; set; }

        [Comment("Author")]
        public virtual Author Author { get; set; } = null!;

        public virtual ICollection<BookCategory> BooksCategories { get; set; } = new HashSet<BookCategory>();

        public virtual ICollection<Edition> Editions { get; set; } = new HashSet<Edition>();

        public virtual ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();

        public virtual ICollection<LendedBook> LendedBooks { get; set; } = new HashSet<LendedBook>();
    }
}
