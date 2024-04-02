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
                        ImageFilePath = "img/Authors/Stephen-Hawking-Image.jpg",
                        IsDeleted = false,
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
                        ImageFilePath = "img/Authors/Jules-Verne-Image.jpg",
                        IsDeleted = false,
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
                        ImageFilePath = "img/Authors/Charles-Dickens-Image.jpg",
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
                        ImageFilePath = "img/Authors/Georgi-Gospodinov-Image.jpg",
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
                        ImageFilePath = "img/Authors/Frank-Herbert-Image.jpg",
                        IsDeleted = false,
                    },
                    new Author
                    {
                        Id = Guid.Parse("1B019738-D4EA-4D18-84D4-F0C2A7F5AAAF"),
                        FirstName = "Gregory",
                        LastName = "Roberts ",
                        Biography = "Roberts reportedly became addicted to heroin after his marriage ended and he lost custody of his young daughter. To finance his drug habit, he turned to crime, becoming known as the Building Society Bandit [2] and the Gentleman Bandit, because he only robbed institutions with adequate insurance. He wore a three-piece suit, and he always said please and thank you to the people he robbed. At the time, Roberts believed that his manner lessened the brutality of his acts but, later in his life, he admitted that people only gave him money because he had made them afraid. He escaped from Pentridge Prison in 1980.[1][5] In 1990, Roberts was captured in Frankfurt, trying to smuggle himself into the country. He was extradited to Australia and served a further six years in prison, two of which were spent in solitary confinement. According to Roberts, he escaped prison again during that time but thought better of it and smuggled himself back into jail. His intention was to serve the rest of his sentence to give himself the chance to be reunited with his family. During his second stay in Australian prison, he began writing Shantaram. The manuscript was destroyed twice by prison staff while Roberts was writing it.",
                        BirthDate = DateOnly.ParseExact("21.06.1952", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Nationality = "Australian",
                        ImageFilePath = "img/Authors/Gregory-Roberts-Image.jpg",
                        IsDeleted = false,
                    },
             };

            await dbContext.Authors.AddRangeAsync(authors);
            await dbContext.SaveChangesAsync();
        }
    }
}