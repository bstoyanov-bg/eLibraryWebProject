using System.ComponentModel.DataAnnotations;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Book;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;

namespace LibraryManagementSystem.Web.ViewModels.Book
{
    public class BookFormModel
    {
        public BookFormModel()
        {
            this.Authors  = new HashSet<AuthorSelectForBookFormModel>();
            this.Categories  = new HashSet<CategorySelectForBookFormModel>();
        }

        [Required]
        [StringLength(TitleMaxLength, ErrorMessage = "Title must be between 2 and 22 characters long.",
        MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [RegularExpression(ISBNRegexPattern)]
        [StringLength(ISBNMaxLength, ErrorMessage = "ISBN number must be max 22 characters long.")]
        public string? ISBN { get; set; }

        [DisplayFormat(DataFormatString = GlobalYearFormat)]
        public DateOnly? YearPublished { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, ErrorMessage = "Description must be between 50 and 1500 characters long.",
        MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [StringLength(PublisherMaxLength, ErrorMessage = "Publisher name must be between 5 and 100 characters long.",
        MinimumLength = PublisherMinLength)]
        public string? Publisher { get; set; } = null!;

        [StringLength(CoverImagePathUrlMaxLength)]
        public string? CoverImagePathUrl { get; set; } = null!;

        public string AuthorId { get; set; } = null!;

        public int CategoryId { get; set; }

        public IEnumerable<AuthorSelectForBookFormModel> Authors { get; set; } = null!;

        public IEnumerable<CategorySelectForBookFormModel> Categories { get; set; } = null!;
    }
}
