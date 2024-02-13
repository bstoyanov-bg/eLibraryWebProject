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
        [Comment("Primary key")]
        [Key]
        public Guid Id { get; set; }

        [Comment("Name of the Author")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("Biography of the Author")]
        [MaxLength(BiographyMaxLength)]
        public string? Biography { get; set; }

        [Comment("Birth date of the Author")]
        public DateOnly? BirthDate { get; set; }

        [Comment("Death date of the Author")]
        public DateOnly? DeathDate { get; set; }

        [Comment("Nationality of the Author")]
        [MaxLength(NationalityMaxLength)]
        public string? Nationality { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}