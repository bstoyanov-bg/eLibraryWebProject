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
        }

        [Comment("Fist name of the user (Member)")]
        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Comment("Last name of the user (Member)")]
        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Comment("Address of the user (Member)")]
        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = null!;

        // TODO --> check and add more properties
        [Comment("Maximum number of books allowed to have at the same time")]
        [Required]
        [MaxLength(AllowedBooksMaxLength)]
        public int MaxLoanedBooks { get; set; }
    }
}
