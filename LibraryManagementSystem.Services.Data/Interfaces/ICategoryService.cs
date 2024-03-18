﻿using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.ViewModels.Category;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface ICategoryService
    {
        // ready
        Task AddCategoryAsync(CategoryFormModel categoryModel);

        Task EditCategoryAsync(int categoryId, CategoryFormModel categoryModel);

        // ready
        Task DeleteCategoryAsync(int categoryId);

        // ready
        Task<Category?> GetCategoryByIdAsync(int categoryId);

        // ready
        Task<CategoryFormModel?> GetCategoryForEditByIdAsync(int categoryId);

        // ready
        Task<bool> CategoryExistByNameAsync(string categoryName);

        // ready
        Task<bool> CategoryExistByIdAsync(int categoryId);

        // ready
        Task<int> GetCategoryIdByBookIdAsync(string bookId);

        // ready
        Task<IEnumerable<AllCategoriesViewModel>> GetAllCategoriesAsync();

        // ready
        Task<IEnumerable<string>> GetAllCategoriesNamesAsync();
    }
}
