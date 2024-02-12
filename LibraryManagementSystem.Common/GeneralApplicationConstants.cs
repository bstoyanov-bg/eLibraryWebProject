namespace LibraryManagementSystem.Common
{
    public static class GeneralApplicationConstants
    {
        public const int ReleaseYear = 2024;

        public const string AdminAreaName = "Admin";
        public const string DevelopmentAdminEmail = "admin@elibrary.bg";

        // This is the Maximum number of books allowed user (Member) to have at the same time.
        // At the moment this number is hardcoded value, but in future a Subscription system will be developed to handle allowed number of books.
        public const int MaxNumberOfBooksAllowed = 10;
    }
}
