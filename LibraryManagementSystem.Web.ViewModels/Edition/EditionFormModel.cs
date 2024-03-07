using LibraryManagementSystem.Web.ViewModels.Author;
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
            Books = new HashSet<BookSelectForEditionFormModel>();
        }

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

        public string BookId { get; set; } = null!;

        public IEnumerable<BookSelectForEditionFormModel> Books { get; set; }
    }
}
