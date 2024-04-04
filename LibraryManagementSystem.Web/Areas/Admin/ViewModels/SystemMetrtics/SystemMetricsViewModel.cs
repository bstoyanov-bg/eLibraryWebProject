namespace LibraryManagementSystem.Web.Areas.Admin.ViewModels.SystemMetrtics
{
    public class SystemMetricsViewModel
    {
        public int ActiveUsers { get; set; }

        public int ActiveAdmins { get; set; }

        public int DeletedUsers { get; set; }


        public int ActiveBooks { get; set; }

        public int ActiveAuthors { get; set; }

        public int ActiveCategories { get; set; }

        public int ActiveBookEditions { get; set; }


        public int DeletedBooks { get; set; }

        public int DeletedAuthors { get; set; }

        public int DeletedCategories { get; set; }

        public int DeletedBookEditions { get; set; }


        public int BookFiles { get; set; }

        public int EditionFiles { get; set; }

        public int BookCovers { get; set; }

        public int AuthorCovers { get; set; }


        public int AllGivenRatings { get; set; }

        public int TotalBooksLended { get; set; }
    }
}
