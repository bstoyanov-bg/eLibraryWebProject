﻿namespace LibraryManagementSystem.Web.ViewModels.LendedBook
{
    public class MyBooksViewModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public DateOnly? YearPublished { get; set; }

        public string? Publisher { get; set; }

        public string AuthorName { get; set; } = null!;

        public string Category { get; set; } = null!;

        public string? ImageFilePath { get; set; }

        public int EditionsCount { get; set; }

        public string? FilePath { get; set; }
    }
}
