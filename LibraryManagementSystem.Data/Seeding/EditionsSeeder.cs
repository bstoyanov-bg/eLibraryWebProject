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
                        FilePath = "BookFiles/Editions/Brief-Answers-to-the-Big-Questions-Version-1.txt",
                        IsDeleted = false,
                    },
                    new Edition
                    {
                        Id = Guid.Parse("C9F8D9F9-5C46-4C7B-9D7E-9F5F5A5F5F5F"),
                        BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                        Version = "Version 2",
                        Publisher = "Hodder",
                        EditionYear = DateOnly.ParseExact("2022", "yyyy", CultureInfo.InvariantCulture),
                        FilePath = "BookFiles/Editions/Brief-Answers-to-the-Big-Questions-Version-2.txt",
                        IsDeleted = false,
                    },
                    new Edition
                    {
                        Id = Guid.Parse("300802D8-99CA-4A6A-86C2-ED8194C3D280"),
                        BookId = Guid.Parse("50E9B56F-9BC1-4356-AC0C-E3D5945778BA"),
                        Version = "Ver 1.0",
                        Publisher = "Pierre-Jules Hetzel",
                        EditionYear = DateOnly.ParseExact("1879", "yyyy", CultureInfo.InvariantCulture),
                        FilePath = "BookFiles/Editions/Journey-to-the-Center-of-the-Earth-Ver-1.txt",
                        IsDeleted = false,
                    },
                    new Edition
                    {
                        Id = Guid.Parse("A42D9106-0B58-4840-AD38-AD50DC0ACDC9"),
                        BookId = Guid.Parse("42CBCCE4-349B-4D8C-A077-318A07BA74CC"),
                        Version = "Version 2.0",
                        Publisher = "Ace",
                        EditionYear = DateOnly.ParseExact("2019", "yyyy", CultureInfo.InvariantCulture),
                        FilePath = "BookFiles/Editions/Journey-to-the-Center-of-the-Earth-Version-2.txt",
                        IsDeleted = false,
                    },
                    new Edition
                    {
                        Id = Guid.Parse("FCBA5560-E156-4B1E-B20C-44E2AE02FEC6"),
                        BookId = Guid.Parse("4C757EFD-2D1D-42A9-8460-88B9E1FFCC7D"),
                        Version = "Version 3.0",
                        Publisher = "Penguin Books",
                        EditionYear = DateOnly.ParseExact("2003", "yyyy", CultureInfo.InvariantCulture),
                        FilePath = "BookFiles/Editions/Oliver-Twist-Version-3.txt",
                        IsDeleted = false,
                    },
                    new Edition
                    {
                        Id = Guid.Parse("6A7083A7-7622-4500-81FF-629EA8AF6CEC"),
                        BookId = Guid.Parse("6232A651-266B-45BC-B652-9696553914B7"),
                        Version = "Ver 2.0",
                        Publisher = "Scribe",
                        EditionYear = DateOnly.ParseExact("2004", "yyyy", CultureInfo.InvariantCulture),
                        FilePath = "BookFiles/Editions/Shantaram-Ver-2.txt",
                        IsDeleted = false,
                    },
                };

            await dbContext.Editions.AddRangeAsync(editions);
            await dbContext.SaveChangesAsync();
        }
    }
}
