﻿using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.ViewModels.LendedBooks;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface ILendedBooksService
    {
        Task AddBookToCollectionAsync(string userId, Book book);

        Task<bool> DoesUserHaveBookInCollectionAsync(string userId, string bookId);

        Task<IEnumerable<MyBooksViewModel>> GetMyBooksAsync(string userId);

        Task<bool> IsBookAddedToUserCollectionAsync(string userId, string bookId);

        Task<int> GetCountOfActiveBooksForUserAsync(string userId);

        Task<bool> IsBookReturnedAsync(string userId, string bookId);
    }
}
