using LibraryManagementSystem.Web.ViewModels.Edition;
using LibraryManagementSystem.Web.ViewModels.Rating;

namespace LibraryManagementSystem.Web.ViewModels.Book
{
    public class BookDetailsViewModel
    {
        public BookDetailsViewModel()
        {
            this.Editions = new HashSet<EditionsForBookDetailsViewModel>();
            this.Comments = new HashSet<CommentViewModel>();
        }

        public string Id { get; set; } = null!;

        public string? ISBN { get; set; }

        public string Title { get; set; } = null!;

        public DateOnly? YearPublished { get; set; }

        public string Description { get; set; } = null!;

        public string? Publisher { get; set; }

        public string? ImageFilePath { get; set; }

        public string AuthorName { get; set; } = null!;

        public string AuthorId { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public decimal Rating { get; set; }

        public int PeopleReading { get; set; }

        public IEnumerable<EditionsForBookDetailsViewModel> Editions { get; set; } = null!;

        public IEnumerable<CommentViewModel> Comments { get; set; } = null!;
    }
}
