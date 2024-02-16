using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.ApplicationUser;

namespace LibraryManagementSystem.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Model representing Custom User (ApplicationUser) which is going to be one of the main users of the application.
        /// </summary>

        public ApplicationUser()
        {
            
            this.Id = Guid.NewGuid();

            this.LendedBooks = new HashSet<LendedBook>();
            this.Ratings = new HashSet<Rating>();
        }

        [Comment("Fist name of the user (Member)")]
        [Required]
        [MaxLength(FirstNameMaxLength)]
        [ProtectedPersonalData]
        public string FirstName { get; set; } = null!;

        [Comment("Last name of the user (Member)")]
        [Required]
        [MaxLength(LastNameMaxLength)]
        [ProtectedPersonalData]
        public string LastName { get; set; } = null!;

        [Comment("Date of birth of the user (Member)")]
        [Required]
        [DisplayFormat(DataFormatString = DateFormat)]
        [ProtectedPersonalData]
        public DateOnly DateOfBirth { get; set; }

        [Comment("Address of the user (Member)")]
        [Required]
        [MaxLength(AddressMaxLength)]
        [ProtectedPersonalData]
        public string Address { get; set; } = null!;

        [Comment("Country of the user (Member)")]
        [Required]
        [MaxLength(CountryMaxLength)]
        public string Country { get; set; } = null!;

        [Comment("City of the user (Member)")]
        [Required]
        [MaxLength(CityMaxLength)]
        public string City { get; set; } = null!;

        [Comment("Maximum number of books allowed to have at the same time")]
        [MaxLength(AllowedBooksMaxLength)]
        public int? MaxLoanedBooks { get; set; }

        public virtual ICollection<LendedBook> LendedBooks { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
