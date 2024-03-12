using System.ComponentModel;
using LibraryManagementSystem.Web.ViewModels.Book.Enums;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;

namespace LibraryManagementSystem.Web.ViewModels.Book
{
    public class AllBooksQueryModel
    {
        public AllBooksQueryModel()
        {
            this.CurrentPage = DefaultPage;
            this.BooksPerPage = EntitiesPerPage;

            this.Categories = new HashSet<string>();
            this.Books = new HashSet<AllBooksViewModel>();
        }

        [DisplayName(displayName: "Filter by Category:")]
        public string? Category { get; set; }

        [DisplayName(displayName: "Search by Book Title or Author:")]
        public string? SearchString { get; set; }

        [DisplayName(displayName: "Sort books by:")]
        public BookSorting BookSorting { get; set; }

        public int CurrentPage { get; set; }

        [DisplayName(displayName: "Books per page:")]
        public int BooksPerPage { get; set; }

        public int TotalBooks { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<AllBooksViewModel> Books { get; set; } = null!;
    }
}
