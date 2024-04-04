using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.Areas.Admin.Services.Interfaces;
using LibraryManagementSystem.Web.Areas.Admin.ViewModels.SystemMetrtics;
using IUserService = LibraryManagementSystem.Web.Areas.Admin.Services.Interfaces.IUserService;

namespace LibraryManagementSystem.Web.Areas.Admin.Services
{
    public class SystemMetricsService : ISystemMetricsService
    {
        private readonly IUserService userService;
        private readonly IBookService bookService;
        private readonly IAuthorService authorService;
        private readonly ICategoryService categoryService;
        private readonly IEditionService editionService;
        private readonly IRatingService ratingService;
        private readonly ILendedBooksService lendedBooksService;
        private readonly IFileService fileService;

        public SystemMetricsService(IUserService userService,
                                    IBookService bookService,
                                  IAuthorService authorService,
                                ICategoryService categoryService,
                                 IEditionService editionService,
                                  IRatingService ratingService,
                             ILendedBooksService lendedBooksService,
                                    IFileService fileService)
        {
            this.userService = userService;
            this.bookService = bookService;
            this.authorService = authorService;
            this.categoryService = categoryService;
            this.editionService = editionService;
            this.ratingService = ratingService;
            this.lendedBooksService = lendedBooksService;
            this.fileService = fileService;
        }

        public async Task<SystemMetricsViewModel> GetSystemMetricsAsync()
        {
            // Users
            int activeUsers = await this.userService.GetCountOfActiveUsersAsync();
            int activeAdmins = await this.userService.GetCountOfActiveAdminsAsync();
            int deletedUsers = await this.userService.GetCountOfDeletedUsersAsync();

            // Active Entities
            int activeBooks = await this.bookService.GetCountOfActiveBooksAsync();
            int activeAuthors = await this.authorService.GetCountOfActiveAuthorsAsync();
            int activeCategories = await this.categoryService.GetCountOfActiveCategoriesAsync();
            int activeBookEditions = await this.editionService.GetCountOfActiveEditionsAsync();

            // Deleted Entities
            int deletedBooks = await this.bookService.GetCountOfDeletedBooksAsync();
            int deletedAuthors = await this.authorService.GetCountOfDeletedAuthorsAsync();
            int deletedCategories = await this.categoryService.GetCountOfDeletedCategoriesAsync();
            int deletedBookEditions = await this.editionService.GetCountOfDeletedEditionsAsync();

            // Library Files
            int bookFiles = await this.fileService.GetFilesCountAsync("BookFiles/Books");
            int eitionFiles = await this.fileService.GetFilesCountAsync("BookFiles/Editions");
            int bookImageFiles = await this.fileService.GetFilesCountAsync("img/BookCovers");
            int authorImageFiles = await this.fileService.GetFilesCountAsync("img/AuthorCovers");

            // Other Information
            int allRatings = await this.ratingService.GetCountOfRatingsAsync();
            int allLendedBooks = await this.lendedBooksService.GetCountOfLendedBooksAsync();

            SystemMetricsViewModel model = new SystemMetricsViewModel
            {
                ActiveUsers = activeUsers,
                ActiveAdmins = activeAdmins,
                DeletedUsers = deletedUsers,

                BookFiles = bookFiles,
                EditionFiles = eitionFiles,
                BookCovers = bookImageFiles,
                AuthorCovers = authorImageFiles,

                ActiveBooks = activeBooks,
                ActiveAuthors = activeAuthors,
                ActiveCategories = activeCategories,
                ActiveBookEditions = activeBookEditions,

                DeletedBooks = deletedBooks,
                DeletedAuthors = deletedAuthors,
                DeletedCategories = deletedCategories,
                DeletedBookEditions = deletedBookEditions,

                AllGivenRatings = allRatings,
                TotalBooksLended = allLendedBooks,
            };

            return model;
        }
    }
}
