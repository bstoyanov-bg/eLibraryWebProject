﻿using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Services.Data.Models.Book;
using LibraryManagementSystem.Web.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.NotificationMessageConstants;
using static LibraryManagementSystem.Common.UserRoleNames;

namespace LibraryManagementSystem.Web.Controllers
{
    public class BookController : BaseController
    {
        private readonly IBookService bookService;
        private readonly ICategoryService categoryService;

        public BookController(IBookService bookService, ICategoryService categoryService) 
        {
            this.bookService = bookService;
            this.categoryService = categoryService;
        }

        // NOT USED ANYMORE
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> All()
        //{
        //    IEnumerable<AllBooksViewModel> viewModel = await bookService.GetAllBooksAsync();

        //    return View(viewModel);
        //}

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]AllBooksQueryModel queryModel)
        {
            AllBooksFilteredAndPagedServiceModel serviceModel = await this.bookService.GetAllBooksFilteredAndPagedAsync(queryModel);

            queryModel.Books = serviceModel.Books;
            queryModel.TotalBooks = serviceModel.TotalBooksCount;
            queryModel.Categories = await this.categoryService.GetAllCategoriesNamesAsync();

            return this.View(queryModel);
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add()
        {
            try
            {
                BookFormModel model = await bookService.GetNewCreateBookModelAsync();

                return View(model);
            }
            catch
            {
                return GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add(BookFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await bookService.AddBookAsync(model);
                TempData[SuccessMessage] = "Succesfully added book";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with adding the book!";
            }

            return this.RedirectToAction("All", "Book");
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var book = await bookService.GetBookForEditByIdAsync(id);

                if (book == null)
                {
                    return RedirectToAction("All", "Book");
                }

                return View(book);
            }
            catch
            {
                return GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id, BookFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var book = await bookService.GetBookForEditByIdAsync(id);

                if (book == null)
                {
                    TempData[ErrorMessage] = "There is no book with such id!";
                }

                await bookService.EditBookAsync(id, model);
                TempData[SuccessMessage] = "Succesfully edited book";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with editing the book!";
            }

            return this.RedirectToAction("All", "Book");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                BookDetailsViewModel book = await bookService.GetBookDetailsForUserAsync(id);

                if (book == null)
                {
                    return RedirectToAction("All", "Book");
                }

                return View(book);
            }
            catch
            {
                return GeneralError();
            }
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await bookService.DeleteBookAsync(id);
                TempData[SuccessMessage] = "Succesfully deleted book";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with deleting the book!";
            }

            return this.RedirectToAction("All", "Book");
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> AddFile(string id)
        {
            try
            {
                var book = await bookService.GetBookForEditByIdAsync(id);

                if (book == null)
                {
                    return RedirectToAction("All", "Book");
                }

                return View(book);
            }
            catch
            {
                return GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> AddFile(string id, BookFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var book = await bookService.GetBookForEditByIdAsync(id);

                if (book == null)
                {
                    TempData[ErrorMessage] = "There is no book with such id!";
                }

                await bookService.EditBookAsync(id, model);
                TempData[SuccessMessage] = "Succesfully added file to the book!";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with editing the book!";
            }

            return this.RedirectToAction("All", "Book");
        }

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return RedirectToAction("Index", "Home");
        }
    }
}

