using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data.Models
{
    /// <summary>
    /// This class represents the mapping between the Book and Category entities.
    /// </summary>

    public class BookCategory
    {
        [Comment("BookId")]
        [Required]
        [ForeignKey(nameof(Book))]
        public Guid BookId { get; set; }
        [Comment("Book")]
        public Book Book { get; set; } = null!;

        [Comment("CategoryId")]
        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        [Comment("Category")]
        public Category Category { get; set; } = null!;
    }
}