using System.ComponentModel.DataAnnotations;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Rating;

namespace LibraryManagementSystem.Web.ViewModels.Rating
{
    public class RatingFormModel
    {
        [Required]
        [MaxLength(BookRatingMaxLength)]
        public decimal BookRating { get; set; }

        [Required]
        [StringLength(CommentMaxLength, ErrorMessage = "Comment must be between 5 and 1000 characters long.",
            MinimumLength = CommentMinLength)]
        public string Comment { get; set; } = null!;

        public string BookId { get; set; } = null!;
    }
}
