namespace LibraryManagementSystem.Web.ViewModels.Book
{
    public class BookSelectForEditionFormModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string AuthorName { get; set; } = null!;

        public string? YearPublished { get; set; }

        public string? Publisher { get; set; } = null!;
    }
}
