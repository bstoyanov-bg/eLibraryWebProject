using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.ViewModels.Category;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public class CategoryService : ICategoryService
    {
        private readonly ELibraryDbContext dbContext;

        public CategoryService(ELibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // TO DO Implement CreateCategoryAsync correct

        public async Task CreateCategoryAsync(AddCategoryInputModel addCategoryInputModel)
        {
            // Validate input
            if (addCategoryInputModel == null)
            {
                throw new ArgumentNullException(nameof(addCategoryInputModel));
            }

            // Create a new Category object
            var category = new Category
            {
                Name = addCategoryInputModel.Name
            };

            // Assuming you have some repository or data access layer to save the category
            await dbContext.AddAsync(category);


            // Return the newly created category
            return;
        }
    }
}
