namespace LibraryManagementSystem.Web.ViewModels.Author
{
    public class AllAuthorsViewModel
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Nationality { get; set; } = null!;

        public int BooksCount { get; set; }
    }
}
