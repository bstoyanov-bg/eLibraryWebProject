namespace LibraryManagementSystem.Web.ViewModels.Rating
{
    public class CommentViewModel
    {
        public decimal BookRating { get; set; }

        public string BookComment { get; set; } = null!;

        public string BookId { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string Username { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}
