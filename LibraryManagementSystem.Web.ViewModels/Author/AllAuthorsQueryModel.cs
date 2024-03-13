using LibraryManagementSystem.Web.ViewModels.Author.Enums;
using System.ComponentModel;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;

namespace LibraryManagementSystem.Web.ViewModels.Author
{
    public class AllAuthorsQueryModel
    {
        public AllAuthorsQueryModel()
        {
            this.CurrentPage = DefaultPage;
            this.AuthorsPerPage = EntitiesPerPage;

            this.Authors = new HashSet<AllAuthorsViewModel>();
        }

        [DisplayName(displayName: "Search by Author:")]
        public string? SearchString { get; set; }

        [DisplayName(displayName: "Sort books by:")]
        public AuthorSorting AuthorSorting { get; set; }

        public int CurrentPage { get; set; }

        [DisplayName(displayName: "Authors per page:")]
        public int AuthorsPerPage { get; set; }

        public int TotalAuthors { get; set; }

        public IEnumerable<AllAuthorsViewModel> Authors { get; set; } = null!;
    }
}
