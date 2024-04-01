using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Web.ViewModels.Category
{
    public class AllCategoriesViewModel
    {
        [Required(ErrorMessage = "Please select at least one author.")]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int BooksCount { get; set; }
    }
}
