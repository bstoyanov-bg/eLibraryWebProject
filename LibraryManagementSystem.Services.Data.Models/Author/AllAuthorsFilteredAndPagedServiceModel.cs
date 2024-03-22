using LibraryManagementSystem.Web.ViewModels.Author;

namespace LibraryManagementSystem.Services.Data.Models.Author
{
    public class AllAuthorsFilteredAndPagedServiceModel
    {
        public AllAuthorsFilteredAndPagedServiceModel()
        {
            this.Authors = new HashSet<AllAuthorsViewModel>();
        }

        public int TotalAuthorsCount { get; set; }

        public IEnumerable<AllAuthorsViewModel> Authors { get; set; }
    }
}
