namespace LibraryManagementSystem.Data.Seeding.Contracts
{
    public interface ISeeder
    {
        Task SeedAsync(ELibraryDbContext dbContext, IServiceProvider serviceProvider);
    }
}
