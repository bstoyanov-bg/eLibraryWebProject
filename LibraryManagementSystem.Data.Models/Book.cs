using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Book;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;

namespace LibraryManagementSystem.Data.Models
{
    /// <summary>
    /// Model representing book entity used in the database.
    /// </summary>
    
    public class Book
    {
        public Book() 
        {
            this.Id = new Guid();

            this.BooksCategories = new HashSet<BookCategory>();
            this.Editions = new HashSet<Edition>();
            this.Ratings = new HashSet<Rating>();
            this.LendedBooks = new HashSet<LendedBook>();
        }

        [Comment("Primary key")]
        [Key]
        public Guid Id { get; set; }

        [Comment("International Standard Book Number")]
        [RegularExpression(ISBNRegexPattern)]
        [MaxLength(ISBNMaxLength)]
        public string? ISBN { get; set; }

        [Comment("Title of the book")]
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Comment("The year of book publish")]
        [MaxLength(YearPublishedMaxLength)]
        [DisplayFormat(DataFormatString = GlobalYearFormat)]
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

        [Comment("Created On")]
        public DateTime CreatedOn { get; set; }

        [Comment("AuthorId")]
        [Required]
        [ForeignKey(nameof(Author))]
        public Guid AuthorId { get; set; }

        [Comment("Author")]
        public virtual Author Author { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public virtual ICollection<BookCategory> BooksCategories { get; set; }

        public virtual ICollection<Edition> Editions { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public virtual ICollection<LendedBook> LendedBooks { get; set; }
    }
}
