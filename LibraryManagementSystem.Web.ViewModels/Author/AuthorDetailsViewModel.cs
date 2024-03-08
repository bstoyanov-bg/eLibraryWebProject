using LibraryManagementSystem.Web.ViewModels.Book;

namespace LibraryManagementSystem.Web.ViewModels.Author
{
    public class AuthorDetailsViewModel
    {
        public AuthorDetailsViewModel()
        {
            Books = new HashSet<BooksForAuthorDetailsFormModel>();
        }

        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Biography { get; set; }

        public DateOnly? BirthDate { get; set; }

        public DateOnly? DeathDate { get; set; }

        public string Nationality { get; set; } = null!;

        public IEnumerable<BooksForAuthorDetailsFormModel> Books { get; set; } = null!;
    }
}
