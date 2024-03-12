using LibraryManagementSystem.Web.ViewModels.Book;

namespace LibraryManagementSystem.Services.Data.Models.Book
{
    public class AllBooksFilteredAndPagedServiceModel
    {
        public AllBooksFilteredAndPagedServiceModel()
        {
            this.Books = new HashSet<AllBooksViewModel>();
        }

        public int TotalBooksCount { get; set; }

        public IEnumerable<AllBooksViewModel> Books { get; set; }
    }
}
