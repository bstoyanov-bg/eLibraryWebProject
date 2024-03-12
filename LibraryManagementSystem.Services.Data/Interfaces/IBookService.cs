﻿using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Models.Book;
using LibraryManagementSystem.Web.ViewModels.Book;
using LibraryManagementSystem.Web.ViewModels.Home;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<IndexViewModel>> LastTenBooksAsync();

        Task<IEnumerable<AllBooksViewModel>> GetAllBooksAsync();

        Task<Book?> GetBookByIdAsync(string bookId);

        Task<BookFormModel> GetNewCreateBookModelAsync();

        Task AddBookAsync(BookFormModel model);

        Task<BookFormModel?> GetBookForEditByIdAsync(string bookId);

        Task EditBookAsync(string bookId, BookFormModel model);

        Task<IEnumerable<BookSelectForEditionFormModel>>GetAllBooksForListAsync();

        Task<BookDetailsViewModel> GetBookDetailsForUserAsync(string bookId);

        Task DeleteBookAsync(string bookId);

        Task<AllBooksFilteredAndPagedServiceModel> GetAllBooksFilteredAndPagedAsync(AllBooksQueryModel queryModel);
    }
}
