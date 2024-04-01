using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Data.Seeding.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data.Seeding
{
    public class LendedBooksSeeder : ISeeder
    {
        public async Task SeedAsync(ELibraryDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.LendedBooks.AnyAsync())
            {
                return;
            }

            IEnumerable<LendedBooks> lendedBooks = new HashSet<LendedBooks>
            {
                new LendedBooks
                {
                    Id = Guid.Parse("19827894-2D91-4778-9338-2EEEA6D8FDCB"),
                    LoanDate = DateTime.UtcNow.AddDays(-60),
                    ReturnDate = DateTime.UtcNow.AddDays(-10),
                    BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                    UserId = Guid.Parse("7F13235C-EAC9-4F60-AA69-BC8FC86FBD24"),
                },
                new LendedBooks
                {
                    Id = Guid.Parse("A49C9AFC-6614-4BA9-BF33-04C9F0E4ACC5"),
                    LoanDate = DateTime.UtcNow.AddDays(-60),
                    BookId = Guid.Parse("50E9B56F-9BC1-4356-AC0C-E3D5945778BA"),
                    UserId = Guid.Parse("7F13235C-EAC9-4F60-AA69-BC8FC86FBD24"),
                },
                new LendedBooks
                {
                    Id = Guid.Parse("6DE0FDA4-77E3-4740-8C07-151F8CFCB211"),
                    LoanDate = DateTime.UtcNow.AddDays(-60),
                    BookId = Guid.Parse("8C6D196A-1E96-4DA0-9B65-91CD7736E13E"),
                    UserId = Guid.Parse("7F13235C-EAC9-4F60-AA69-BC8FC86FBD24"),
                },
                new LendedBooks
                {
                    Id = Guid.Parse("41F7A155-9FDD-42AD-8FA3-05F0BECA3C2C"),
                    LoanDate = DateTime.UtcNow.AddDays(-80),
                    ReturnDate = DateTime.UtcNow.AddDays(-5),
                    BookId = Guid.Parse("DDDED6BD-AAB9-4503-B285-AA2DE7FF7BC3"),
                    UserId = Guid.Parse("7F13235C-EAC9-4F60-AA69-BC8FC86FBD24"),
                },
                new LendedBooks
                {
                    Id = Guid.Parse("09F11E1E-9442-4AB4-9DCE-01108046997A"),
                    LoanDate = DateTime.UtcNow.AddDays(-100),
                    ReturnDate = DateTime.UtcNow.AddDays(-20),
                    BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                    UserId = Guid.Parse("89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153"),
                },
                new LendedBooks
                {
                    Id = Guid.Parse("24CC467B-EBD2-4161-834A-EDE33E2CB5F9"),
                    LoanDate = DateTime.UtcNow.AddDays(-150),
                    ReturnDate = DateTime.UtcNow.AddDays(-50),
                    BookId = Guid.Parse("50E9B56F-9BC1-4356-AC0C-E3D5945778BA"),
                    UserId = Guid.Parse("89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153"),
                },
                new LendedBooks
                {
                    Id = Guid.Parse("E7A42B3B-32C3-4739-92E2-AC61234AF83D"),
                    LoanDate = DateTime.UtcNow.AddDays(-90),
                    BookId = Guid.Parse("DDDED6BD-AAB9-4503-B285-AA2DE7FF7BC3"),
                    UserId = Guid.Parse("89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153"),
                },
                new LendedBooks
                {
                    Id = Guid.Parse("9DBAC327-0033-489C-977D-7A948BEDEAB9"),
                    LoanDate = DateTime.UtcNow.AddDays(-90),
                    ReturnDate = DateTime.UtcNow.AddDays(-5),
                    BookId = Guid.Parse("A6D5D2D7-A6FB-46EF-AA1D-9502A0EF1C50"),
                    UserId = Guid.Parse("89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153"),
                },
                new LendedBooks
                {
                    Id = Guid.Parse("F5E010BE-1A82-43C4-BF40-021B3880FAD0"),
                    LoanDate = DateTime.UtcNow.AddDays(-60),
                    ReturnDate = DateTime.UtcNow.AddDays(-10),
                    BookId = Guid.Parse("D8EBD1EE-C555-418C-844C-28BE14D44314"),
                    UserId = Guid.Parse("BBBDF7F6-94E2-4BE8-A036-84AF63E028FC"),
                },
                new LendedBooks
                {
                    Id = Guid.Parse("81D767F4-7B14-4D30-87B0-139335D979B1"),
                    LoanDate = DateTime.UtcNow.AddDays(-60),
                    ReturnDate = DateTime.UtcNow.AddDays(-10),
                    BookId = Guid.Parse("42CBCCE4-349B-4D8C-A077-318A07BA74CC"),
                    UserId = Guid.Parse("BBBDF7F6-94E2-4BE8-A036-84AF63E028FC"),
                },
                new LendedBooks
                {
                    Id = Guid.Parse("906880F1-C4E0-4F8B-9492-FA45D2BF457E"),
                    LoanDate = DateTime.UtcNow.AddDays(-60),
                    BookId = Guid.Parse("A6D5D2D7-A6FB-46EF-AA1D-9502A0EF1C50"),
                    UserId = Guid.Parse("BBBDF7F6-94E2-4BE8-A036-84AF63E028FC"),
                },
                new LendedBooks
                {
                    Id = Guid.Parse("3E2A42E3-524F-4ECB-9545-88C307C45416"),
                    LoanDate = DateTime.UtcNow.AddDays(-60),
                    BookId = Guid.Parse("50E9B56F-9BC1-4356-AC0C-E3D5945778BA"),
                    UserId = Guid.Parse("BBBDF7F6-94E2-4BE8-A036-84AF63E028FC"),
                },
            };

            await dbContext.LendedBooks.AddRangeAsync(lendedBooks);
            await dbContext.SaveChangesAsync();
        }
    }
}
