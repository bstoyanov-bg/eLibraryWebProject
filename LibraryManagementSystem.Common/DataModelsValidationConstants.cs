namespace LibraryManagementSystem.Common
{
    public static class DataModelsValidationConstants
    {
        public static class Category
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }

        public static class Book
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 150;

            public const int YearPublishedMaxLength = 5;

            public const int DescriptionMinLength = 50;
            public const int DescriptionMaxLength = 1500;

            public const int PublisherMinLength = 5;
            public const int PublisherMaxLength = 100;

            public const int CoverImagePathUrlMaxLength = 2083;

            public const string ISBNRegexPattern = @"\b(?:ISBN(?:-13)?:? ?)?(?=[0-9]{13}$|(?=(?:[0-9]+[- ]){3})[- 0-9]{17}$)[0-9]{1,5}[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9X]\b";
        }

        public static class Author
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 100;

            public const int BiographyMinLength = 10;
            public const int BiographyMaxLength = 1500;

            public const int NationalityMinLength = 2;
            public const int NationalityMaxLength = 50;
        }

        public static class Edition
        {
            public const int VersionMinLength = 1;
            public const int VersionMaxLength = 10;

            public const int PublisherMinLength = 5;
            public const int PublisherMaxLength = 100;

            public const int EditionYearMaxLength = 5;
        }
    }
}
