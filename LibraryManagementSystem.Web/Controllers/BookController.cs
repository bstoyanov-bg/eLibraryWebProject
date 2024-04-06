using Ganss.Xss;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Services.Data.Models.Author;
using LibraryManagementSystem.Services.Data.Models.Book;
using LibraryManagementSystem.Web.ViewModels.Author;
using LibraryManagementSystem.Web.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static LibraryManagementSystem.Common.NotificationMessageConstants;
using static LibraryManagementSystem.Common.UserRoleNames;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;

namespace LibraryManagementSystem.Web.Controllers
{
    public class BookController : BaseController
    {
        private readonly IBookService bookService;
        private readonly ICategoryService categoryService;
        private readonly IFileService fileService;
        private readonly IMemoryCache memoryCache;

        public BookController(IBookService bookService, ICategoryService categoryService, IFileService fileService, IMemoryCache memoryCache)
        {
            this.bookService = bookService;
            this.categoryService = categoryService;
            this.fileService = fileService;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllBooksQueryModel queryModel)
        {
            // Calculate a unique cache key based on the query parameters
            string cacheKey = $"{BooksCacheKey}_{queryModel.CurrentPage}_{queryModel.BooksPerPage}";

            // Attempt to retrieve data from cache
            if (!memoryCache.TryGetValue(cacheKey, out AllBooksFilteredAndPagedServiceModel? cachedData))
            {
                // Data not found in cache, fetch it from the service
                cachedData = await this.bookService.GetAllBooksFilteredAndPagedAsync(queryModel);

                // Cache the fetched data
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(BooksCacheDurationInMinutes));

                this.memoryCache.Set(UsersCacheKey, cachedData, cacheOptions);
            }

            // Populate the query model with cached data
            queryModel.Books = cachedData!.Books;
            queryModel.TotalBooks = cachedData.TotalBooksCount;

            return View(queryModel);
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add()
        {
            try
            {
                BookFormModel model = await bookService.GetCreateNewBookModelAsync();

                return this.View(model);
            }
            catch
            {
                return this.GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add(BookFormModel model, IFormFile? bookImage)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                bool bookExists = await this.bookService.BookExistByTitleAndAuthorIdAsync(model.Title, model.AuthorId);

                if (bookExists)
                {
                    this.TempData[ErrorMessage] = "Book with the same Title and Author already exists!";

                    return this.RedirectToAction("All", "Book");
                }

                if (!string.IsNullOrEmpty(model.ISBN))
                {
                    bool bookISBNExists = await this.bookService.BookExistByISBNAsync(model.ISBN);

                    if (bookISBNExists)
                    {
                        this.TempData[ErrorMessage] = "Book with the same ISBN number already exists!";

                        return this.RedirectToAction("All", "Book");
                    }
                }

                if (model.Description != null)
                {
                    model.Description = new HtmlSanitizer().Sanitize(model.Description);
                }

                var addedBook = await this.bookService.AddBookAsync(model);

                if (bookImage != null)
                {
                    await this.fileService.UploadImageFileAsync(addedBook.Id.ToString(), bookImage, "Book");
                }

                this.memoryCache.Remove(BooksCacheKey);

                this.TempData[SuccessMessage] = $"Successfully added Book.";

                return this.RedirectToAction("Edit", "Book", new { id = addedBook.Id });
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with adding the Book!";
            }

            return this.RedirectToAction("All", "Book");
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                bool bookExists = await this.bookService.BookExistByIdAsync(id);

                if (!bookExists)
                {
                    this.TempData[ErrorMessage] = "Such Book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                BookFormModel? book = await this.bookService.GetBookForEditByIdAsync(id);

                return this.View(book);
            }
            catch
            {
                return this.GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id, BookFormModel model, IFormFile? bookImage)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                bool bookExists = await this.bookService.BookExistByIdAsync(id);

                if (!bookExists)
                {
                    this.TempData[ErrorMessage] = "Such Book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                if (model.Description != null)
                {
                    model.Description = new HtmlSanitizer().Sanitize(model.Description);
                }

                var editedBook = await this.bookService.EditBookAsync(id, model);

                if (bookImage != null)
                {
                    await this.fileService.UploadImageFileAsync(editedBook.Id.ToString(), bookImage, "Book");
                }

                this.memoryCache.Remove(BooksCacheKey);

                this.TempData[SuccessMessage] = "Succesfully edited Book.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with editing the Book!";
            }

            return this.RedirectToAction("Details", "Book", new { id });
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Delete(string id)
        {
            bool bookExists = await this.bookService.BookExistByIdAsync(id);

            if (!bookExists)
            {
                this.TempData[ErrorMessage] = "Such Book does not exists!";

                return this.RedirectToAction("All", "Book");
            }

            try
            {
                await this.bookService.DeleteBookAsync(id);

                this.TempData[SuccessMessage] = "Succesfully deleted Book.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with deleting the book!";
            }

            return this.RedirectToAction("All", "Book");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            bool bookExists = await this.bookService.BookExistByIdAsync(id);

            if (!bookExists)
            {
                this.TempData[ErrorMessage] = "Such Book does not exists!";

                return this.RedirectToAction("All", "Book");
            }

            try
            {
                BookDetailsViewModel book = await this.bookService.GetBookDetailsForUserAsync(id);

                return this.View(book);
            }
            catch
            {
                return this.GeneralError();
            }
        }

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] = "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }
    }
}
