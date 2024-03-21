using LibraryManagementSystem.Web.ViewModels.Edition;

namespace LibraryManagementSystem.Web.ViewModels.Book
{
    public class BookDetailsViewModel
    {
        public BookDetailsViewModel()
        {
            Editions = new HashSet<EditionsForBookDetailsViewModel>();
        }

        public string Id { get; set; } = null!;

        public string? ISBN { get; set; }

        public string Title { get; set; } = null!;

        public DateOnly? YearPublished { get; set; }

        public string Description { get; set; } = null!;

        public string? Publisher { get; set; }

        public string? CoverImagePathUrl { get; set; }

        public string AuthorName { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public decimal Rating { get; set; }

        public IEnumerable<EditionsForBookDetailsViewModel> Editions { get; set; } = null!;
    }
}
