using LibraryManagementSystem.Web.ViewModels.Book;
using System.ComponentModel.DataAnnotations;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Edition;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;

namespace LibraryManagementSystem.Web.ViewModels.Edition
{
    public class EditionFormModel
    {
        public EditionFormModel()
        {
            BooksDropDown = new HashSet<BookSelectForEditionFormModel>();
        }

        // Used for Uplode BookFile in Edit View
        public string? Id { get; set; }

        [Required]
        [StringLength(VersionMaxLength, ErrorMessage = "Version must be between 1 and 10 characters long.",
        MinimumLength = VersionMinLength)]
        public string Version { get; set; } = null!;

        [Required]
        [StringLength(PublisherMaxLength, ErrorMessage = "Publisher must be between 5 and 100 characters long.",
        MinimumLength = PublisherMinLength)]
        public string Publisher { get; set; } = null!;

        [Required]
        [DisplayFormat(DataFormatString = GlobalYearFormat)]
        public DateOnly EditionYear { get; set; }

        [StringLength(FilePathMaxLength)]
        public string? FilePath { get; set; }

        public string BookId { get; set; } = null!;

        public IEnumerable<BookSelectForEditionFormModel> BooksDropDown { get; set; }
    }
}
