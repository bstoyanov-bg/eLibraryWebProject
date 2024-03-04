using System.ComponentModel.DataAnnotations;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Author;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;


namespace LibraryManagementSystem.Web.ViewModels.Author
{
    public class AuthorFormModel
    {
        [Required]
        [StringLength(FirstNameMaxLength, ErrorMessage = "First name must be between 5 and 100 characters long.", 
            MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, ErrorMessage = "Last name must be between 5 and 100 characters long.", 
            MinimumLength = LastNameMinLength)]
        public string LastName { get; set; } = null!;

        [StringLength(BiographyMaxLength, ErrorMessage = "Biography must be between 10 and 1500 characters long.",
            MinimumLength = BiographyMinLength)]
        public string? Biography { get; set; }

        [DisplayFormat(DataFormatString = GlobalYearFormat)]
        public DateOnly? BirthDate { get; set; }

        [DisplayFormat(DataFormatString = GlobalYearFormat)]
        public DateOnly? DeathDate { get; set; }

        [Required]
        [StringLength(NationalityMaxLength, ErrorMessage = "Nationality must be between 2 and 50 characters long.",
            MinimumLength = NationalityMinLength)]
        public string Nationality { get; set; } = null!;
    }
}
