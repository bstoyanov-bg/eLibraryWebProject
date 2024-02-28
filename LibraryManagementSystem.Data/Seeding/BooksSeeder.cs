using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Data.Seeding.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LibraryManagementSystem.Data.Seeding
{
    public class BooksSeeder : ISeeder
    {
        public async Task SeedAsync(ELibraryDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Books.AnyAsync())
            {
                return;
            }

            IEnumerable<Book> books = new HashSet<Book>
            {
                new Book
                {
                    ISBN = "978-3-16-148410-0",
                    Title = "Journey to the Center of the Earth",
                    YearPublished = DateOnly.ParseExact("1874", "yyyy", CultureInfo.InvariantCulture),
                    Description = "Journey to the Center of the Earth (French: Voyage au centre de la Terre), also translated with the variant titles A Journey to the Centre of the Earth and A Journey into the Interior of the Earth, is a classic science fiction novel by Jules Verne. It was first published in French in 1864, then reissued in 1867 in a revised and expanded edition. Professor Otto Lidenbrock is the tale's central figure, an eccentric German scientist who believes there are volcanic tubes that reach to the very center of the earth. He, his nephew Axel, and their Icelandic guide Hans rappel into Iceland's celebrated inactive volcano Snæfellsjökull, then contend with many dangers, including cave-ins, subpolar tornadoes, an underground ocean, and living prehistoric creatures from the Mesozoic and Cenozoic eras (the 1867 revised edition inserted additional prehistoric material in Chaps. 37–39). Eventually the three explorers are spewed back to the surface by an active volcano, Stromboli, located in southern Italy.",
                    Publisher = "unknown",
                    CoverImagePathUrl = "img/BookCovers/Journey_to_the_Center_of_the_Earth_Image.jpg",
                    AuthorId = Guid.Parse("92FB2DF0-9C82-4B71-B1FB-4DE8CA702866"),
                },
                new Book
                {
                    ISBN = "978-3-16-148410-1",
                    Title = "Twenty Thousand Leagues Under the Seas",
                    YearPublished = DateOnly.ParseExact("1872", "yyyy", CultureInfo.InvariantCulture),
                    Description = "The novel was originally serialized from March 1869 through June 1870 in Pierre-Jules Hetzel's fortnightly periodical, the Magasin d'éducation et de récréation. A deluxe octavo edition, published by Hetzel in November 1871, included 111 illustrations by Alphonse de Neuville and Édouard Riou.[2] The book was widely acclaimed on its release and remains so; it is regarded as one of the premier adventure novels and one of Verne's greatest works, along with Around the World in Eighty Days and Journey to the Center of the Earth. Its depiction of Captain Nemo's underwater ship, the Nautilus, is regarded as ahead of its time, since it accurately describes many features of today's submarines, which in the 1860s were comparatively primitive vessels.",
                    CoverImagePathUrl = "img/BookCovers/Twenty_Thousand_Leagues_Under_the_Seas_Image.jpg",
                    AuthorId = Guid.Parse("92FB2DF0-9C82-4B71-B1FB-4DE8CA702866"),
                }
            };

            await dbContext.Books.AddRangeAsync(books);
            await dbContext.SaveChangesAsync();
        }
    }
}
