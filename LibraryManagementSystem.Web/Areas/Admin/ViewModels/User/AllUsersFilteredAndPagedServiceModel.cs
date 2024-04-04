using LibraryManagementSystem.Web.Areas.Admin.ViewModels.User;

namespace LibraryManagementSystem.Services.Data.Models.User
{
    public class AllUsersFilteredAndPagedServiceModel
    {
        public AllUsersFilteredAndPagedServiceModel()
        {
            this.Users = new HashSet<AllUsersViewModel>();
        }

        public int TotalUsersCount { get; set; }

        public IEnumerable<AllUsersViewModel> Users { get; set; }
    }
}
