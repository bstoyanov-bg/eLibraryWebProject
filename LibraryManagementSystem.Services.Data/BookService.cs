using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Book;

namespace LibraryManagementSystem.Services.Data
{
    public class BookService : IBookService
    {
        private readonly ELibraryDbContext dbContext;

        public BookService(ELibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateBookAsync(AddBookInputModel addBookInputModel)
        {
            throw new NotImplementedException();

            //Book book = new Book
            //{
                
            //};
        }
    }
}
    