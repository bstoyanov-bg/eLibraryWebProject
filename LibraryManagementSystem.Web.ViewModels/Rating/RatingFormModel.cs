using System.ComponentModel.DataAnnotations;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Rating;

namespace LibraryManagementSystem.Web.ViewModels.Rating
{
    public class RatingFormModel
    {
        [Required]
        [Range(typeof(decimal), RatingMinValue, RatingMaxValue)]
        [Display(Name = "Give your Rating:")]
        public decimal BookRating { get; set; }

        [Required]
        [StringLength(CommentMaxLength, ErrorMessage = "Comment must be between 5 and 1000 characters long.",
            MinimumLength = CommentMinLength)]
        [Display(Name = "Give your comment:")]
        public string Comment { get; set; } = null!;

        public string BookId { get; set; } = null!;

        public string UserId { get; set; } = null!;
    }
}
