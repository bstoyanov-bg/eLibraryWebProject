namespace LibraryManagementSystem.Common
{
    public static class GeneralApplicationConstants
    {
        public const string AppName = "eLibrary";

        public const int ReleaseYear = 2024;

        public const string AdminAreaName = "Admin";
        public const string DevelopmentAdminEmail = "admin@elibrary.bg";

        public const string TestUserEmail = "testUser@elibrary.bg";

        public const string GlobalDateFormat = "dd.mm.yyyy";
        public const string GlobalDateTimeFormat = "dd.mm.yyyy HH:mm";
        public const string GlobalYearFormat = "yyyy";

        // This is the Maximum number of books allowed user (Member) to have at the same time.
        // At the moment this number is hardcoded value, but in future a Subscription system will be developed to handle allowed number of books.
        public const int MaxNumberOfBooksAllowed = 5;

        public const int DefaultPage = 1;
        public const int EntitiesPerPage = 10;

        //public const string UsersCacheKey = "UsersCache";
        //public const int UsersCacheDurationInMinutes = 5;

        //public const string AuthorsCacheKey = "AuthorsCache";
        //public const int AuthorsCacheDurationInMinutes = 5;

        //public const string BooksCacheKey = "BooksCache";
        //public const int BooksCacheDurationInMinutes = 5;
    }
}
