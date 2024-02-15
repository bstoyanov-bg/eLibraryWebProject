﻿namespace LibraryManagementSystem.Common
{
    /// <summary>
    /// Constants and regular expressions for data model validations.
    /// </summary> 

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
            public const int FirstNameMinLength = 5;
            public const int FirstNameMaxLength = 100;

            public const int LastNameMinLength = 5;
            public const int LastNameMaxLength = 100;

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

        public static class ApplicationUser
        {
            public const int FirstNameMinLength = 2;
            public const int FirstNameMaxLength = 30;

            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 30;

            public const int AddressMinLength = 5;
            public const int AddressMaxLength = 50;

            public const int CountryMinLength = 4;
            public const int CountryMaxLength = 60;

            public const int CityMinLength = 1;
            public const int CityMaxLength = 100;

            public const int AllowedBooksMaxLength = 2;
        }

        public static class Rating
        {
            public const int BookRatingMaxLength = 5;

            public const int CommentMinLength = 5;
            public const int CommentMaxLength = 1000;
        }
    }
}
