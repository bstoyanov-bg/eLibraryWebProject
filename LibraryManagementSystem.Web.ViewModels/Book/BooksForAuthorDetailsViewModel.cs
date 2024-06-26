﻿namespace LibraryManagementSystem.Web.ViewModels.Book
{
    public class BooksForAuthorDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? ISBN { get; set; }

        public DateOnly? YearPublished { get; set; }

        public string Description { get; set; } = null!;

        public string? Publisher { get; set; } = null!;

        public string? ImageFilePath { get; set; } = null!;
    }
}
