using LibraryManagementSystem.Web.Areas.Admin.ViewModels.User.Enums;
using System.ComponentModel;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;

namespace LibraryManagementSystem.Web.Areas.Admin.ViewModels.User
{
    public class AllUsersQueryModel
    {
        public AllUsersQueryModel()
        {
            CurrentPage = DefaultPage;
            UsersPerPage = EntitiesPerPage;

            Users = new HashSet<AllUsersViewModel>();
        }

        [DisplayName(displayName: "Search by Username or Email:")]
        public string? SearchString { get; set; }

        [DisplayName(displayName: "Sort users by:")]
        public UserSorting UserSorting { get; set; }

        public int CurrentPage { get; set; }

        [DisplayName(displayName: "Users per page:")]
        public int UsersPerPage { get; set; }

        public int TotalUsers { get; set; }

        public IEnumerable<AllUsersViewModel> Users { get; set; } = null!;
    }
}
