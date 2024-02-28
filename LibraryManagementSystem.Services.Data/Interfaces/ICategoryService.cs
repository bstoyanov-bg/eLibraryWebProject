﻿using LibraryManagementSystem.Web.ViewModels.Category;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryFormModel model);

        Task<IEnumerable<AllCategoriesViewModel>> GetAllCategoriesAsync();

        Task<CategoryFormModel?> GetCategoryForEditByIdAsync(int categoryId);

        Task EditCategoryAsync(int categoryId, CategoryFormModel model);
    }
}
