namespace LibraryManagementSystem.Web.ViewModels.Category
{
    public class AllCategoriesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int BooksCount { get; set; }
    }
}
