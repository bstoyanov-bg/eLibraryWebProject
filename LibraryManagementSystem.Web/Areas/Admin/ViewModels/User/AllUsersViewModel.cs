namespace LibraryManagementSystem.Web.Areas.Admin.ViewModels.User
{
    public class AllUsersViewModel
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string City { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public int? MaxLoanedBooks { get; set; }
    }
}
