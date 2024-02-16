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
            Book book = new Book
            {
                ManagerId = Guid.Parse(managerId),
                Name = createGymInputModel.Name,
                Email = createGymInputModel.Email,
                PhoneNumber = createGymInputModel.PhoneNumber,
                Description = createGymInputModel.Description,
                LogoUri = createGymInputModel.LogoResultParams!.SecureUri!.AbsoluteUri,
                LogoPublicId = createGymInputModel.LogoResultParams.PublicId,
                WebsiteUrl = createGymInputModel.WebsiteUrl,
                GymType = Enum.Parse<GymType>(createGymInputModel.GymType),
                CreatedOn = DateTime.UtcNow
            };
        }
    }
}
    