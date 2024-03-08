namespace LibraryManagementSystem.Web.ViewModels.Book
{
    public class BooksForAuthorDetailsFormModel
    {
        public string Title { get; set; } = null!;

        public string? ISBN { get; set; }

        public DateOnly? YearPublished { get; set; }

        public string Description { get; set; } = null!;

        public string? Publisher { get; set; } = null!;

        public string? CoverImagePathUrl { get; set; } = null!;

        //public string CategoryName { get; set; } = null!;
    }
}
