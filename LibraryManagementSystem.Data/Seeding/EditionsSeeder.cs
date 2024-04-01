using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Data.Seeding.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LibraryManagementSystem.Data.Seeding
{
    public class EditionsSeeder : ISeeder
    {
        public async Task SeedAsync(ELibraryDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Editions.AnyAsync())
            {
                return;
            }

            IEnumerable<Edition> editions = new HashSet<Edition>
                {
                    new Edition
                    {
                        Id = Guid.Parse("1394B818-A8A6-474F-B123-5AD206C5F49D"),
                        BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                        Version = "Version 1",
                        Publisher = "Hodder & Stoughton",
                        EditionYear = DateOnly.ParseExact("2018", "yyyy", CultureInfo.InvariantCulture),
                        FilePath = "BookFiles/Editions/Brief-Answers-to-the-Big-Questions_V1.txt",
                        IsDeleted = false,
                    },
                    new Edition
                    {
                        Id = Guid.Parse("C9F8D9F9-5C46-4C7B-9D7E-9F5F5A5F5F5F"),
                        BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                        Version = "Version 2",
                        Publisher = "Hodder",
                        EditionYear = DateOnly.ParseExact("2022", "yyyy", CultureInfo.InvariantCulture),
                        FilePath = "BookFiles/Editions/Brief-Answers-to-the-Big-Questions_V2.txt",
                        IsDeleted = false,
                    },
                    new Edition
                    {
                        Id = Guid.Parse("300802D8-99CA-4A6A-86C2-ED8194C3D280"),
                        BookId = Guid.Parse("50E9B56F-9BC1-4356-AC0C-E3D5945778BA"),
                        Version = "Version 1.0",
                        Publisher = "Pierre-Jules Hetzel",
                        EditionYear = DateOnly.ParseExact("1879", "yyyy", CultureInfo.InvariantCulture),
                        FilePath = "BookFiles/Editions/Journey-to-the-Center-of-the-Earth.txt",
                        IsDeleted = false,
                    },
                };

            await dbContext.Editions.AddRangeAsync(editions);
            await dbContext.SaveChangesAsync();
        }
    }
}
