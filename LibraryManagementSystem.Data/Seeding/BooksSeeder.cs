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
                    Id = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                    ISBN = "978-4-0743-2365-4",
                    Title = "Brief Answers to the Big Questions",
                    YearPublished = DateOnly.ParseExact("2018", "yyyy", CultureInfo.InvariantCulture),
                    Description = "Brief Answers to the Big Questions is a popular science book written by physicist Stephen Hawking, and published by Hodder & Stoughton (hardcover) and Bantam Books (paperback) on 16 October 2018. The book examines some of the universe's greatest mysteries, and promotes the view that science is very important in helping to solve problems on planet Earth. The publisher describes the book as \"a selection of [Hawking's] most profound, accessible, and timely reflections from his personal archive\", and is based on, according to a book reviewer, \"half a million or so words\" from his essays, lectures and keynote speeches.",
                    ImageFilePath = "img/BookCovers/Brief_Answers_to_the_Big_Questions.png",
                    AuthorId = Guid.Parse("3CF69D33-43D6-4AD9-8569-0C2A897A66C0"),
                    IsDeleted = false,
                    Editions = new HashSet<Edition>
                    {
                        new Edition
                        {
                            Id = Guid.Parse("1394B818-A8A6-474F-B123-5AD206C5F49D"),
                            BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                            Version = "Version 1",
                            Publisher = "Hodder & Stoughton",
                            EditionYear = DateOnly.ParseExact("2018", "yyyy", CultureInfo.InvariantCulture),
                            FilePath = "BookFiles/Editions/Brief-Answers-to-the-Big-Questions_V1.txt",
                        },
                        new Edition
                        {
                            Id = Guid.Parse("C9F8D9F9-5C46-4C7B-9D7E-9F5F5A5F5F5F"),
                            BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                            Version = "Version 2",
                            Publisher = "Hodder",
                            EditionYear = DateOnly.ParseExact("2022", "yyyy", CultureInfo.InvariantCulture),
                            FilePath = "BookFiles/Editions/Brief-Answers-to-the-Big-Questions_V2.txt",
                        }
                    }
                },
                new Book
                {
                    Id = Guid.Parse("5F14A26F-43EC-46C8-95E9-C1FE18FAC856"),
                    ISBN = "978-9-8176-9173-1",
                    Title = "The Illustrated Theory of Everything: The Origin and Fate of the Universe",
                    YearPublished = DateOnly.ParseExact("2003", "yyyy", CultureInfo.InvariantCulture),
                    Description = "In physicist Stephen Hawking's brilliant opus, A Brief History of Time, he presented us with a bold new look at our universe, how it began, and how our old views of physics and tired theories about the creation of the universe were no longer relevant. In other words, Hawking gave us a new look at our world, our universe, and ourselves. Now, available for the first time in a deluxe full-color edition with never-before-seen photos and illustrations, Hawking presents an even more comprehensive look at our universe, its creation, and how we see ourselves within it. Imagine sitting in a comfortable room listening to Hawking discuss his latest theories and place them in historical context with science's other great achievements--it would be like hearing Christopher Columbus deliver the news about the new world. Hawking presents a series of seven lectures in which he describes, more clearly than ever, the history of the universe as we know it. He begins with the history of ideas about the universe, from Aristotle's idea that the Earth is round to Hubble's discovery two millennia later that our universe is growing.",
                    ImageFilePath = "img/BookCovers/The_Illustrated_Theory_of_Everything_The_Origin_and_Fate_of_the_Universe.jpg",
                    AuthorId = Guid.Parse("3CF69D33-43D6-4AD9-8569-0C2A897A66C0"),
                    IsDeleted = false,
                },
                new Book
                {
                    Id = Guid.Parse("50E9B56F-9BC1-4356-AC0C-E3D5945778BA"),
                    ISBN = "978-3-16-148410-0",
                    Title = "Journey to the Center of the Earth",
                    YearPublished = DateOnly.ParseExact("1874", "yyyy", CultureInfo.InvariantCulture),
                    Description = "Journey to the Center of the Earth (French: Voyage au centre de la Terre), also translated with the variant titles A Journey to the Centre of the Earth and A Journey into the Interior of the Earth, is a classic science fiction novel by Jules Verne. It was first published in French in 1864, then reissued in 1867 in a revised and expanded edition. Professor Otto Lidenbrock is the tale's central figure, an eccentric German scientist who believes there are volcanic tubes that reach to the very center of the earth. He, his nephew Axel, and their Icelandic guide Hans rappel into Iceland's celebrated inactive volcano Snæfellsjökull, then contend with many dangers, including cave-ins, subpolar tornadoes, an underground ocean, and living prehistoric creatures from the Mesozoic and Cenozoic eras (the 1867 revised edition inserted additional prehistoric material in Chaps. 37–39). Eventually the three explorers are spewed back to the surface by an active volcano, Stromboli, located in southern Italy.",
                    Publisher = "Pierre-Jules Hetzel",
                    ImageFilePath = "img/BookCovers/Journey_to_the_Center_of_the_Earth_Image.jpg",
                    AuthorId = Guid.Parse("92FB2DF0-9C82-4B71-B1FB-4DE8CA702866"),
                    IsDeleted = false,
                    Editions = new HashSet<Edition>
                    {
                        new Edition
                        {
                            Id = Guid.Parse("300802D8-99CA-4A6A-86C2-ED8194C3D280"),
                            BookId = Guid.Parse("50E9B56F-9BC1-4356-AC0C-E3D5945778BA"),
                            Version = "Version 1.0",
                            Publisher = "Pierre-Jules Hetzel",
                            EditionYear = DateOnly.ParseExact("1879", "yyyy", CultureInfo.InvariantCulture),
                            FilePath = "BookFiles/Editions/Journey-to-the-Center-of-the-Earth.txt",
                        },
                    }
                },
                new Book
                {
                    Id = Guid.Parse("3B7822CB-17D3-47FE-8C2B-2CA761F376F1"),
                    ISBN = "978-3-16-148410-1",
                    Title = "Twenty Thousand Leagues Under the Seas",
                    YearPublished = DateOnly.ParseExact("1872", "yyyy", CultureInfo.InvariantCulture),
                    Description = "The novel was originally serialized from March 1869 through June 1870 in Pierre-Jules Hetzel's fortnightly periodical, the Magasin d'éducation et de récréation. A deluxe octavo edition, published by Hetzel in November 1871, included 111 illustrations by Alphonse de Neuville and Édouard Riou.[2] The book was widely acclaimed on its release and remains so; it is regarded as one of the premier adventure novels and one of Verne's greatest works, along with Around the World in Eighty Days and Journey to the Center of the Earth. Its depiction of Captain Nemo's underwater ship, the Nautilus, is regarded as ahead of its time, since it accurately describes many features of today's submarines, which in the 1860s were comparatively primitive vessels.",
                    Publisher = "Pierre-Jules Hetzel",
                    ImageFilePath = "img/BookCovers/Twenty_Thousand_Leagues_Under_the_Seas_Image.jpg",
                    AuthorId = Guid.Parse("92FB2DF0-9C82-4B71-B1FB-4DE8CA702866"),
                    IsDeleted = false,
                },
            };

            await dbContext.Books.AddRangeAsync(books);
            await dbContext.SaveChangesAsync();
        }
    }
}
