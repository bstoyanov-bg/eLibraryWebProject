using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Data.Seeding.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LibraryManagementSystem.Data.Seeding
{
    public class AuthorsSeeder : ISeeder
    {
        public async Task SeedAsync(ELibraryDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Authors.AnyAsync())
            {
                return;
            }

            IEnumerable<Author> authors = new HashSet<Author>
                {
                    new Author
                    {
                        Id = Guid.Parse("3CF69D33-43D6-4AD9-8569-0C2A897A66C0"),
                        FirstName = "Stephen",
                        LastName = "Hawking",
                        Biography = "Stephen William Hawking was an English theoretical physicist, cosmologist, and author who was director of research at the Centre for Theoretical Cosmology at the University of Cambridge. Between 1979 and 2009, he was the Lucasian Professor of Mathematics at Cambridge, widely viewed as one of the most prestigious academic posts in the world.\r\n\r\nHawking was born in Oxford into a family of physicians. In October 1959, at the age of 17, he began his university education at University College, Oxford, where he received a first-class BA degree in physics. In October 1962, he began his graduate work at Trinity Hall, Cambridge, where, in March 1966, he obtained his PhD degree in applied mathematics and theoretical physics, specialising in general relativity and cosmology.",
                        BirthDate = DateOnly.ParseExact("08.01.1942", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        DeathDate = DateOnly.ParseExact("14.03.2018", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Nationality = "English",
                        IsDeleted = false,
                        Books = new HashSet<Book>
                        {
                            new Book
                            {
                                Id = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                                ISBN = "978-4-0743-2365-4",
                                Title = "Brief Answers to the Big Questions",
                                YearPublished = DateOnly.ParseExact("2018", "yyyy", CultureInfo.InvariantCulture),
                                Description = "Brief Answers to the Big Questions is a popular science book written by physicist Stephen Hawking, and published by Hodder & Stoughton (hardcover) and Bantam Books (paperback) on 16 October 2018. The book examines some of the universe's greatest mysteries, and promotes the view that science is very important in helping to solve problems on planet Earth. The publisher describes the book as \"a selection of [Hawking's] most profound, accessible, and timely reflections from his personal archive\", and is based on, according to a book reviewer, \"half a million or so words\" from his essays, lectures and keynote speeches.",
                                CoverImagePathUrl = "img/BookCovers/BriefAnswersToTheBigQuestions-BookCover.jpg",
                                AuthorId = Guid.Parse("3CF69D33-43D6-4AD9-8569-0C2A897A66C0"),
                                IsDeleted = false,
                            },
                            new Book
                            {
                                Id = Guid.Parse("5F14A26F-43EC-46C8-95E9-C1FE18FAC856"),
                                ISBN = "978-9-8176-9173-1",
                                Title = "The Illustrated Theory of Everything: The Origin and Fate of the Universe",
                                YearPublished = DateOnly.ParseExact("2003", "yyyy", CultureInfo.InvariantCulture),
                                Description = "In physicist Stephen Hawking's brilliant opus, A Brief History of Time, he presented us with a bold new look at our universe, how it began, and how our old views of physics and tired theories about the creation of the universe were no longer relevant. In other words, Hawking gave us a new look at our world, our universe, and ourselves. Now, available for the first time in a deluxe full-color edition with never-before-seen photos and illustrations, Hawking presents an even more comprehensive look at our universe, its creation, and how we see ourselves within it. Imagine sitting in a comfortable room listening to Hawking discuss his latest theories and place them in historical context with science's other great achievements--it would be like hearing Christopher Columbus deliver the news about the new world. Hawking presents a series of seven lectures in which he describes, more clearly than ever, the history of the universe as we know it. He begins with the history of ideas about the universe, from Aristotle's idea that the Earth is round to Hubble's discovery two millennia later that our universe is growing.",
                                CoverImagePathUrl = "img/BookCovers/The-Illustrated-Theory-of-EverythingThe-Origin-and-Fate-of-the-Universe.jpg",
                                AuthorId = Guid.Parse("3CF69D33-43D6-4AD9-8569-0C2A897A66C0"),
                                IsDeleted = false,
                            }
                        }
                    },
                    new Author
                    {
                        Id = Guid.Parse("92FB2DF0-9C82-4B71-B1FB-4DE8CA702866"),
                        FirstName = "Jules",
                        LastName = "Verne",
                        Biography = "Jules Gabriel Verne was a French novelist, poet, and playwright. His collaboration with the publisher Pierre-Jules Hetzel led to the creation of the Voyages extraordinaires, a series of bestselling adventure novels including Journey to the Center of the Earth (1864), Twenty Thousand Leagues Under the Seas (1870), and Around the World in Eighty Days (1872). His novels, always well documented, are generally set in the second half of the 19th century, taking into account the technological advances of the time.",
                        BirthDate = DateOnly.ParseExact("08.02.1828", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        DeathDate = DateOnly.ParseExact("24.03.1905", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Nationality = "French",
                        IsDeleted = false,
                        Books = new HashSet<Book>
                        {
                            new Book
                            {
                                Id = Guid.Parse("50E9B56F-9BC1-4356-AC0C-E3D5945778BA"),
                                ISBN = "978-3-16-148410-0",
                                Title = "Journey to the Center of the Earth",
                                YearPublished = DateOnly.ParseExact("1874", "yyyy", CultureInfo.InvariantCulture),
                                Description = "Journey to the Center of the Earth (French: Voyage au centre de la Terre), also translated with the variant titles A Journey to the Centre of the Earth and A Journey into the Interior of the Earth, is a classic science fiction novel by Jules Verne. It was first published in French in 1864, then reissued in 1867 in a revised and expanded edition. Professor Otto Lidenbrock is the tale's central figure, an eccentric German scientist who believes there are volcanic tubes that reach to the very center of the earth. He, his nephew Axel, and their Icelandic guide Hans rappel into Iceland's celebrated inactive volcano Snæfellsjökull, then contend with many dangers, including cave-ins, subpolar tornadoes, an underground ocean, and living prehistoric creatures from the Mesozoic and Cenozoic eras (the 1867 revised edition inserted additional prehistoric material in Chaps. 37–39). Eventually the three explorers are spewed back to the surface by an active volcano, Stromboli, located in southern Italy.",
                                Publisher = "Pierre-Jules Hetzel",
                                CoverImagePathUrl = "img/BookCovers/Journey_to_the_Center_of_the_Earth_Image.jpg",
                                AuthorId = Guid.Parse("92FB2DF0-9C82-4B71-B1FB-4DE8CA702866"),
                                IsDeleted = false,
                            },
                            new Book
                            {
                                Id = Guid.Parse("3B7822CB-17D3-47FE-8C2B-2CA761F376F1"),
                                ISBN = "978-3-16-148410-1",
                                Title = "Twenty Thousand Leagues Under the Seas",
                                YearPublished = DateOnly.ParseExact("1872", "yyyy", CultureInfo.InvariantCulture),
                                Description = "The novel was originally serialized from March 1869 through June 1870 in Pierre-Jules Hetzel's fortnightly periodical, the Magasin d'éducation et de récréation. A deluxe octavo edition, published by Hetzel in November 1871, included 111 illustrations by Alphonse de Neuville and Édouard Riou.[2] The book was widely acclaimed on its release and remains so; it is regarded as one of the premier adventure novels and one of Verne's greatest works, along with Around the World in Eighty Days and Journey to the Center of the Earth. Its depiction of Captain Nemo's underwater ship, the Nautilus, is regarded as ahead of its time, since it accurately describes many features of today's submarines, which in the 1860s were comparatively primitive vessels.",
                                Publisher = "Pierre-Jules Hetzel",
                                CoverImagePathUrl = "img/BookCovers/Twenty_Thousand_Leagues_Under_the_Seas_Image.jpg",
                                AuthorId = Guid.Parse("92FB2DF0-9C82-4B71-B1FB-4DE8CA702866"),
                                IsDeleted = false,
                            }
                        }
                    },
                    new Author
                    {
                        Id = Guid.Parse("1BAAD29E-BB6A-4424-95AE-9DCF01FA6712"),
                        FirstName = "Charles ",
                        LastName = "Dickens",
                        Biography = "Charles John Huffam Dickens was an English novelist and social critic who created some of the world's best-known fictional characters, and is regarded by many as the greatest novelist of the Victorian era. His works enjoyed unprecedented popularity during his lifetime and, by the 20th century, critics and scholars had recognised him as a literary genius. His novels and short stories are widely read today.",
                        BirthDate = DateOnly.ParseExact("07.02.1812", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        DeathDate = DateOnly.ParseExact("09.06.1870", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Nationality = "English",
                        IsDeleted = false,
                    },
                    new Author
                    {
                        Id = Guid.Parse("5992BC8E-87F8-41AF-9EFB-E2C354E4BD3B"),
                        FirstName = "Georgi",
                        LastName = "Gospodinov",
                        Biography = "Georgi Gospodinov Georgiev is a Bulgarian writer, poet and playwright. His novel Time Shelter received the 2023 International Booker Prize, shared with translator Angela Rodel, as well as the Strega European Prize. His novel The Physics of Sorrow received the Jan Michalski Prize and the Angelus Award. His works have been translated into 25 languages.",
                        BirthDate = DateOnly.ParseExact("07.01.1968", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Nationality = "Bulgarian",
                        IsDeleted = false,
                    },
                    new Author
                    {
                        Id = Guid.Parse("7A774B0B-D493-4A9D-AB57-CCCEDC6DDCB8"),
                        FirstName = "Franklin",
                        LastName = "Herbert",
                        Biography = "Franklin Patrick Herbert Jr. was an American science fiction author best known for the 1965 novel Dune and its five sequels. Though he became famous for his novels, he also wrote short stories and worked as a newspaper journalist, photographer, book reviewer, ecological consultant, and lecturer.\r\n\r\nThe Dune saga, set in the distant future, and taking place over millennia, explores complex themes, such as the long-term survival of the human species, human evolution, planetary science and ecology, and the intersection of religion, politics, economics and power in a future where humanity has long since developed interstellar travel and settled many thousands of worlds. Dune is the best-selling science fiction novel of all time, and the entire series is considered to be among the classics of the genre.",
                        BirthDate = DateOnly.ParseExact("08.10.1920", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        DeathDate = DateOnly.ParseExact("11.02.1986", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Nationality = "American",
                        IsDeleted = false,
                    },
                };

            await dbContext.Authors.AddRangeAsync(authors);
            await dbContext.SaveChangesAsync();
        }
    }
}