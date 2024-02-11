using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Category;

namespace LibraryManagementSystem.Data.Models
{
    /// <summary>
    /// Model representing category entity for the books used in the database.
    /// </summary>

    public class Category
    {
        [Comment("Primary key")]
        [Key]
        public int Id { get; set; }

        [Comment("Name of the Category")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<BookCategory> BooksCategories { get; set; } = new List<BookCategory>();
    }
}
