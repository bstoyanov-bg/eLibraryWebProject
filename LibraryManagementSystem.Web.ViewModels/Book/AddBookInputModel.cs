using System.ComponentModel.DataAnnotations;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Book;

namespace LibraryManagementSystem.Web.ViewModels.Book
{
    public class AddBookInputModel
    {
        [RegularExpression(ISBNRegexPattern, ErrorMessage = "ISBN must follow this pattern")]
        [Display(Name = "International Standard Book Number")]
        [StringLength(ISBNMaxLength, ErrorMessage = "ISBN number must be max 22 characters long.")]
        public string? ISBN { get; set; }

        [Required]
        [Display(Name = "Title of the Book")]
        [StringLength(TitleMaxLength, ErrorMessage = "Title must be between 2 and 22 characters long.",
        MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Display(Name = "The year of book publish")]
        [DisplayFormat(DataFormatString = DateFormat)]
        public DateOnly? YearPublished { get; set; }

        [Required]
        [Display(Name = "Description of the book")]
        [StringLength(DescriptionMaxLength, ErrorMessage = "Description must be between 50 and 1500 characters long.",
        MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Display(Name = "Publisher of the book")]
        [StringLength(PublisherMaxLength, ErrorMessage = "Publisher name must be between 5 and 100 characters long.",
        MinimumLength = PublisherMinLength)]
        public string? Publisher { get; set; } = null!;

        [Display(Name = "Cover image of the book")]
        [StringLength(CoverImagePathUrlMaxLength)]
        public string? CoverImagePathUrl { get; set; } = null!;

        //[Required]
        //public Guid AuthorId { get; set; }
    }
}
