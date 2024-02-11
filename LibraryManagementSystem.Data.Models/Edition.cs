using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static LibraryManagementSystem.Common.DataModelsValidationConstants.Edition;

namespace LibraryManagementSystem.Data.Models
{
    public class Edition
    {
        [Comment("Primary key")]
        [Key]
        public Guid Id { get; set; }

        [Comment("Version of the edition")]
        [Required]
        [MaxLength(VersionMaxLength)]
        public string Version { get; set; } = null!;

        [Comment("Publisher of the book")]
        [Required]
        [MaxLength(PublisherMaxLength)]
        public string Publisher { get; set; } = null!;

        [Comment("The year of book edition")]
        [Required]
        [MaxLength(EditionYearMaxLength)]
        public DateOnly EditionYear { get; set; }

        [Comment("BookId")]
        [Required]
        [ForeignKey(nameof(Book))]
        public Guid BookId { get; set; }

        [Comment("Book")]
        public virtual Book Book { get; set; } = null!;
    }
}
