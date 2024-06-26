﻿namespace LibraryManagementSystem.Web.ViewModels.Book
{
    public class AllBooksViewModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public DateOnly? YearPublished { get; set; }

        public string? Publisher { get; set; }

        public string AuthorName { get; set; } = null!;

        public string Category { get; set; } = null!;

        public string? ImageFilePath { get; set; }

        public int EditionsCount { get; set; }
    }
}
