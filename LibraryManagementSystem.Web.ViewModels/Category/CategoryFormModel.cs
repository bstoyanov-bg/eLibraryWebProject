using System.ComponentModel.DataAnnotations;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Category;

namespace LibraryManagementSystem.Web.ViewModels.Category
{
    public class CategoryFormModel
    {
        [Required]
        [StringLength(NameMaxLength, ErrorMessage = "The name must be between 2 and 50 characters long.", 
            MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;
    }
}
