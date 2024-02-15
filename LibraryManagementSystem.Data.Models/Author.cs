using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Author;

namespace LibraryManagementSystem.Data.Models
{
    /// <summary>
    /// Model representing author entity for the books used in the database.
    /// </summary>

    public class Author
    {
        public Author()
        {
            this.Id = Guid.NewGuid();

            this.Books = new HashSet<Book>();
        }

        [Comment("Primary key")]
        [Key]
        public Guid Id { get; set; }

        [Comment("First name of the Author")]
        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Comment("Last name of the Author")]
        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Comment("Biography of the Author")]
        [MaxLength(BiographyMaxLength)]
        public string? Biography { get; set; }

        [Comment("Birth date of the Author")]
        public DateOnly? BirthDate { get; set; }

        [Comment("Death date of the Author")]
        public DateOnly? DeathDate { get; set; }

        [Comment("Nationality of the Author")]
        [Required]
        [MaxLength(NationalityMaxLength)]
        public string Nationality { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}