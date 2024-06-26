﻿using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using System.Globalization;

namespace LibraryManagementSystem.Services.Tests
{
    public static class DatabaseSeeder
    {
        public static Author? FirstAuthor;
        public static Author? SecondAuthor;
        public static Author? ThirdAuthor;
        public static Author? ForthAuthor;
        public static Author? FifthAuthor;

        public static Book? FirstBook;
        public static Book? SecondBook;
        public static Book? ThirdBook;
        public static Book? ForthBook;
        public static Book? FifthBook;
        public static Book? SixthBook;
        public static Book? SeventhBook;
        public static Book? EigthBook;
        public static Book? NinthBook;

        public static Category? FirstCategory;
        public static Category? SecondCategory;
        public static Category? ThirdCategory;
        public static Category? ForthCategory;
        public static Category? FifthCategory;
        public static Category? SixthCategory;

        public static Edition? FirstEdition;
        public static Edition? SecondEdition;
        public static Edition? ThirdEdition;

        public static Rating? FirstRating;
        public static Rating? SecondRating;
        public static Rating? ThirdRating;

        public static LendedBook? FirstLendedBook;
        public static LendedBook? SecondLendedBook;
        public static LendedBook? ThirdLendedBook;
        public static LendedBook? ForthLendedBook;
        public static LendedBook? FifthLendedBook;

        public static ApplicationUser? FirstUser;
        public static ApplicationUser? SecondUser;
        public static ApplicationUser? ThirdUser;
        public static ApplicationUser? ForthUser;

        public static void SeedDatabase(ELibraryDbContext dbContext)
        {
            // AUTHORS
            FirstAuthor = new Author()
            {
                Id = Guid.Parse("3CF69D33-43D6-4AD9-8569-0C2A897A66C0"),
                FirstName = "Stephen",
                LastName = "Hawking",
                Biography = "Stephen William Hawking was an English theoretical physicist, cosmologist, and author who was director of research at the Centre for Theoretical Cosmology at the University of Cambridge. Between 1979 and 2009, he was the Lucasian Professor of Mathematics at Cambridge, widely viewed as one of the most prestigious academic posts in the world.\r\n\r\nHawking was born in Oxford into a family of physicians. In October 1959, at the age of 17, he began his university education at University College, Oxford, where he received a first-class BA degree in physics. In October 1962, he began his graduate work at Trinity Hall, Cambridge, where, in March 1966, he obtained his PhD degree in applied mathematics and theoretical physics, specialising in general relativity and cosmology.",
                BirthDate = DateOnly.ParseExact("08.01.1942", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                DeathDate = DateOnly.ParseExact("14.03.2018", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Nationality = "English",
                ImageFilePath = "img/AuthorCovers/c3894951-11e7-458d-a86c-44548d2d57c1_Stephen-Hawking-Image.jpg",
                IsDeleted = false,
            };

            SecondAuthor = new Author()
            {
                Id = Guid.Parse("5992BC8E-87F8-41AF-9EFB-E2C354E4BD3B"),
                FirstName = "Georgi",
                LastName = "Gospodinov",
                Biography = "Georgi Gospodinov Georgiev is a Bulgarian writer, poet and playwright. His novel Time Shelter received the 2023 International Booker Prize, shared with translator Angela Rodel, as well as the Strega European Prize. His novel The Physics of Sorrow received the Jan Michalski Prize and the Angelus Award. His works have been translated into 25 languages.",
                BirthDate = DateOnly.ParseExact("07.01.1968", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Nationality = "Bulgarian",
                ImageFilePath = "img/AuthorCovers/a5ffb32a-eeb8-416b-a0bd-9d5a59f433f5_Georgi-Gospodinov-Image.jpg",
                IsDeleted = false,
            };

            ThirdAuthor = new Author()
            {
                Id = Guid.Parse("7A774B0B-D493-4A9D-AB57-CCCEDC6DDCB8"),
                FirstName = "Franklin",
                LastName = "Herbert",
                Biography = "Franklin Patrick Herbert Jr. was an American science fiction author best known for the 1965 novel Dune and its five sequels. Though he became famous for his novels, he also wrote short stories and worked as a newspaper journalist, photographer, book reviewer, ecological consultant, and lecturer.\r\n\r\nThe Dune saga, set in the distant future, and taking place over millennia, explores complex themes, such as the long-term survival of the human species, human evolution, planetary science and ecology, and the intersection of religion, politics, economics and power in a future where humanity has long since developed interstellar travel and settled many thousands of worlds. Dune is the best-selling science fiction novel of all time, and the entire series is considered to be among the classics of the genre.",
                BirthDate = DateOnly.ParseExact("08.10.1920", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                DeathDate = DateOnly.ParseExact("11.02.1986", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Nationality = "American",
                ImageFilePath = "img/AuthorCovers/11da75c5-3f40-4256-9785-dd9101af213b_Frank-Herbert-Image.jpg",
                IsDeleted = false,
            };

            ForthAuthor = new Author()
            {
                Id = Guid.Parse("92FB2DF0-9C82-4B71-B1FB-4DE8CA702866"),
                FirstName = "Jules",
                LastName = "Verne",
                Biography = "Jules Gabriel Verne was a French novelist, poet, and playwright. His collaboration with the publisher Pierre-Jules Hetzel led to the creation of the Voyages extraordinaires, a series of bestselling adventure novels including Journey to the Center of the Earth (1864), Twenty Thousand Leagues Under the Seas (1870), and Around the World in Eighty Days (1872). His novels, always well documented, are generally set in the second half of the 19th century, taking into account the technological advances of the time.",
                BirthDate = DateOnly.ParseExact("08.02.1828", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                DeathDate = DateOnly.ParseExact("24.03.1905", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Nationality = "French",
                ImageFilePath = "img/AuthorCovers/25f09293-8de6-4184-b293-c21284006a28_Jules-Verne-Image.jpg",
                IsDeleted = false,
            };

            FifthAuthor = new Author()
            {
                Id = Guid.Parse("1BAAD29E-BB6A-4424-95AE-9DCF01FA6712"),
                FirstName = "Charles ",
                LastName = "Dickens",
                Biography = "Charles John Huffam Dickens was an English novelist and social critic who created some of the world's best-known fictional characters, and is regarded by many as the greatest novelist of the Victorian era. His works enjoyed unprecedented popularity during his lifetime and, by the 20th century, critics and scholars had recognised him as a literary genius. His novels and short stories are widely read today.",
                BirthDate = DateOnly.ParseExact("07.02.1812", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                DeathDate = DateOnly.ParseExact("09.06.1870", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Nationality = "English",
                ImageFilePath = "img/AuthorCovers/5c2d2ec2-dfb8-47bd-89d2-66b35ea1a93e_Charles-Dickens-Image.jpg",
                IsDeleted = false,
            };

            // BOOKS
            FirstBook = new Book()
            {
                Id = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                ISBN = "978-4-0743-2365-4",
                Title = "Brief Answers to the Big Questions",
                YearPublished = DateOnly.ParseExact("2018", "yyyy", CultureInfo.InvariantCulture),
                Description = "Brief Answers to the Big Questions is a popular science book written by physicist Stephen Hawking, and published by Hodder & Stoughton (hardcover) and Bantam Books (paperback) on 16 October 2018. The book examines some of the universe's greatest mysteries, and promotes the view that science is very important in helping to solve problems on planet Earth. The publisher describes the book as \"a selection of [Hawking's] most profound, accessible, and timely reflections from his personal archive\", and is based on, according to a book reviewer, \"half a million or so words\" from his essays, lectures and keynote speeches.",
                ImageFilePath = "img/BookCovers/3a740213-ae68-43ea-8d78-79d0656581a7_Brief-Answers-to-the-Big-Questions-Image.png",
                AuthorId = Guid.Parse("3CF69D33-43D6-4AD9-8569-0C2A897A66C0"),
                FilePath = "BookFiles/Books/f4f985e7-0431-428f-91ab-e7d585a192cd_Brief-Answers-to-the-Big-Questions.txt",
                CreatedOn = DateTime.UtcNow.AddDays(10),
                IsDeleted = false,
            };

            SecondBook = new Book()
            {
                Id = Guid.Parse("5F14A26F-43EC-46C8-95E9-C1FE18FAC856"),
                ISBN = "978-9-8176-9173-1",
                Title = "The Illustrated Theory of Everything: The Origin and Fate of the Universe",
                YearPublished = DateOnly.ParseExact("2003", "yyyy", CultureInfo.InvariantCulture),
                Description = "In physicist Stephen Hawking's brilliant opus, A Brief History of Time, he presented us with a bold new look at our universe, how it began, and how our old views of physics and tired theories about the creation of the universe were no longer relevant. In other words, Hawking gave us a new look at our world, our universe, and ourselves. Now, available for the first time in a deluxe full-color edition with never-before-seen photos and illustrations, Hawking presents an even more comprehensive look at our universe, its creation, and how we see ourselves within it. Imagine sitting in a comfortable room listening to Hawking discuss his latest theories and place them in historical context with science's other great achievements--it would be like hearing Christopher Columbus deliver the news about the new world. Hawking presents a series of seven lectures in which he describes, more clearly than ever, the history of the universe as we know it. He begins with the history of ideas about the universe, from Aristotle's idea that the Earth is round to Hubble's discovery two millennia later that our universe is growing.",
                ImageFilePath = "img/BookCovers/7fd490a9-c011-42cd-9d76-fd86a79f0f20_The-Illustrated-Theory-of-Everything-The-Origin-and-Fate-of-the-Universe-Image.jpg",
                AuthorId = Guid.Parse("3CF69D33-43D6-4AD9-8569-0C2A897A66C0"),
                FilePath = "BookFiles/Books/0cc5d03b-7a83-47b8-9d53-fe5a604c83b3_The-Illustrated-Theory-of-Everything-The-Origin-and-Fate-of-the-Universe.txt",
                CreatedOn = DateTime.UtcNow.AddDays(-25),
                IsDeleted = false,
            };

            ThirdBook = new Book()
            {
                Id = Guid.Parse("7DF9AE98-DBB6-4498-9ED5-3C6F19641CBF"),
                ISBN = "957-4-51-1853170-2",
                Title = "Time Shelter",
                YearPublished = DateOnly.ParseExact("2020", "yyyy", CultureInfo.InvariantCulture),
                Description = "Award-winning Bulgarian author Georgi Gospodinov has enthralled readers around the world with his labyrinth-like, Kafkaesque tales of contemporary Europe. In Time Shelter, an enigmatic flâneur named Gaustine opens a “clinic for the past” that offers a promising treatment for Alzheimer’s sufferers: each floor reproduces a decade in minute detail, transporting patients back in time. As Gaustine’s assistant, the unnamed narrator is tasked with collecting the flotsam and jetsam of the past, from 1960s furniture and 1940s shirt buttons to scents and even afternoon light. But as the rooms become more convincing, an increasing number of healthy people seek out the clinic as a “time shelter”—a development that results in an unexpected conundrum when the past begins to invade the present. Intricately crafted, and eloquently translated by Angela Rodel, Time Shelter announces Gospodinov to American readers as an essential voice in international literature.",
                Publisher = "Colibri",
                ImageFilePath = "img/BookCovers/51254e99-8b95-45d2-b77c-0795d2b7d3de_Time-Shelter-Image.jpg",
                AuthorId = Guid.Parse("5992BC8E-87F8-41AF-9EFB-E2C354E4BD3B"),
                FilePath = "BookFiles/Books/e7d5d9d3-66c8-4245-913f-4c4a9d6cfb8e_Time-Shelter.txt",
                CreatedOn = DateTime.UtcNow.AddDays(-80),
                IsDeleted = false,
            };

            ForthBook = new Book()
            {
                Id = Guid.Parse("DDDED6BD-AAB9-4503-B285-AA2DE7FF7BC3"),
                ISBN = "917-4-41-1852140-2",
                Title = "The Physics of Sorrow",
                YearPublished = DateOnly.ParseExact("2011", "yyyy", CultureInfo.InvariantCulture),
                Description = "A quirky, compulsively readable book that deftly hints at the emptiness and sadness at its core. New York Times A finalist for both the Strega Europeo and Gregor von Rezzori awards (and winner of every Bulgarian honor possible), The Physics of Sorrow reaffirms Georgi Gospodinov's place as one of Europe's most inventive and daring writers. Using the myth of the Minotaur as its organizing image, the narrator of Gospodinov's long-awaited novel constructs a labyrinth of stories about his family, jumping from era to era and viewpoint to viewpoint, exploring the mindset and trappings of Eastern Europeans. Incredibly moving such as with the story of his grandfather accidentally being left behind at a mill and extraordinarily funny see the section on the awfulness of the question how are you?Physics is a book that you can inhabit, tracing connections, following the narrator down various side passages, getting pleasantly lost in the various stories and empathizing with the sorrowful, misunderstood Minotaur at the center of it all.",
                Publisher = "Colibri",
                ImageFilePath = "img/BookCovers/b67e1511-e323-410b-9f35-dff659ae6ba8_The-Physics-of-Sorrow-Image.jpg",
                AuthorId = Guid.Parse("5992BC8E-87F8-41AF-9EFB-E2C354E4BD3B"),
                FilePath = "BookFiles/Books/0be734d0-ea25-474b-8655-8901d7f59aef_The-Physics-of-Sorrow.txt",
                CreatedOn = DateTime.UtcNow.AddDays(30),
                IsDeleted = false,
            };

            FifthBook = new Book()
            {
                Id = Guid.Parse("42CBCCE4-349B-4D8C-A077-318A07BA74CC"),
                ISBN = "485-4-33-1827140-7",
                Title = "Dune",
                YearPublished = DateOnly.ParseExact("1965", "yyyy", CultureInfo.InvariantCulture),
                Description = "Set on the desert planet Arrakis, Dune is the story of the boy Paul Atreides, heir to a noble family tasked with ruling an inhospitable world where the only thing of value is the “spice” melange, a drug capable of extending life and enhancing consciousness. Coveted across the known universe, melange is a prize worth killing for... When House Atreides is betrayed, the destruction of Paul’s family will set the boy on a journey toward a destiny greater than he could ever have imagined. And as he evolves into the mysterious man known as Muad’Dib, he will bring to fruition humankind’s most ancient and unattainable dream.",
                Publisher = "Ace",
                ImageFilePath = "img/BookCovers/eeccd4fa-e95a-4e10-bdd4-e774441fcfca_Dune-Image.jpg",
                AuthorId = Guid.Parse("7A774B0B-D493-4A9D-AB57-CCCEDC6DDCB8"),
                FilePath = "BookFiles/Books/da9dc514-06a1-40ec-b143-d4a35b664f47_Dune.txt",
                CreatedOn = DateTime.UtcNow.AddDays(20),
                IsDeleted = false,
            };

            SixthBook = new Book()
            {
                Id = Guid.Parse("D8EBD1EE-C555-418C-844C-28BE14D44314"),
                ISBN = "957-1-87-1824443-7",
                Title = "Children of Dune",
                YearPublished = DateOnly.ParseExact("1976", "yyyy", CultureInfo.InvariantCulture),
                Description = "Children of Dune is a 1976 science fiction novel by Frank Herbert, the third in his Dune series of six novels. It was originally serialized in Analog Science Fiction and Fact in 1976, and was the last Dune novel to be serialized before book publication. At the end of Dune Messiah, Paul Atreides walks into the desert, a blind man, leaving his twin children Leto and Ghanima in the care of the Fremen, while his sister Alia rules the universe as regent. Awakened in the womb by the spice, the children are the heirs to Paul's prescient vision of the fate of the universe, a role that Alia desperately craves. House Corrino schemes to return to the throne, while the Bene Gesserit make common cause with the Tleilaxu and Spacing Guild to gain control of the spice and the children of Paul Atreides.",
                Publisher = "Putnam",
                ImageFilePath = "img/BookCovers/59b9a300-8e8a-466e-886f-a57b24d09a02_Children-of-Dune-Image.jpg",
                AuthorId = Guid.Parse("7A774B0B-D493-4A9D-AB57-CCCEDC6DDCB8"),
                FilePath = "BookFiles/Books/b6402aeb-614f-4181-874f-ec76f24fc4a5_Children-of-Dune.txt",
                CreatedOn = DateTime.UtcNow.AddDays(2),
                IsDeleted = false,
            };

            SeventhBook = new Book()
            {
                Id = Guid.Parse("50E9B56F-9BC1-4356-AC0C-E3D5945778BA"),
                ISBN = "978-3-16-148410-0",
                Title = "Journey to the Center of the Earth",
                YearPublished = DateOnly.ParseExact("1874", "yyyy", CultureInfo.InvariantCulture),
                Description = "Journey to the Center of the Earth (French: Voyage au centre de la Terre), also translated with the variant titles A Journey to the Centre of the Earth and A Journey into the Interior of the Earth, is a classic science fiction novel by Jules Verne. It was first published in French in 1864, then reissued in 1867 in a revised and expanded edition. Professor Otto Lidenbrock is the tale's central figure, an eccentric German scientist who believes there are volcanic tubes that reach to the very center of the earth. He, his nephew Axel, and their Icelandic guide Hans rappel into Iceland's celebrated inactive volcano Snæfellsjökull, then contend with many dangers, including cave-ins, subpolar tornadoes, an underground ocean, and living prehistoric creatures from the Mesozoic and Cenozoic eras (the 1867 revised edition inserted additional prehistoric material in Chaps. 37–39). Eventually the three explorers are spewed back to the surface by an active volcano, Stromboli, located in southern Italy.",
                Publisher = "Pierre-Jules Hetzel",
                ImageFilePath = "img/BookCovers/3d8b729f-4be0-4134-b756-c44f54b7a7f1_Journey-to-the-Center-of-the-Earth-Image.jpg",
                AuthorId = Guid.Parse("92FB2DF0-9C82-4B71-B1FB-4DE8CA702866"),
                FilePath = "BookFiles/Books/285d8783-3491-4643-9b5f-a9bdd8a49271_Journey-to-the-Center-of-the-Earth.txt",
                CreatedOn = DateTime.UtcNow.AddDays(1),
                IsDeleted = false,
            };

            EigthBook = new Book()
            {
                Id = Guid.Parse("3B7822CB-17D3-47FE-8C2B-2CA761F376F1"),
                ISBN = "978-3-16-148410-1",
                Title = "Twenty Thousand Leagues Under the Seas",
                YearPublished = DateOnly.ParseExact("1872", "yyyy", CultureInfo.InvariantCulture),
                Description = "The novel was originally serialized from March 1869 through June 1870 in Pierre-Jules Hetzel's fortnightly periodical, the Magasin d'éducation et de récréation. A deluxe octavo edition, published by Hetzel in November 1871, included 111 illustrations by Alphonse de Neuville and Édouard Riou.[2] The book was widely acclaimed on its release and remains so; it is regarded as one of the premier adventure novels and one of Verne's greatest works, along with Around the World in Eighty Days and Journey to the Center of the Earth. Its depiction of Captain Nemo's underwater ship, the Nautilus, is regarded as ahead of its time, since it accurately describes many features of today's submarines, which in the 1860s were comparatively primitive vessels.",
                Publisher = "Pierre-Jules Hetzel",
                ImageFilePath = "img/BookCovers/df61a4e7-b8dc-48b7-bb19-2d4beeae5902_Twenty-Thousand-Leagues-Under-the-Seas-Image.jpg",
                AuthorId = Guid.Parse("92FB2DF0-9C82-4B71-B1FB-4DE8CA702866"),
                FilePath = "BookFiles/Books/e04ad026-7d9d-4c6e-97b1-c1ca4a5efbc3_Twenty-Thousand-Leagues-Under-the-Seas.txt",
                CreatedOn = DateTime.UtcNow.AddDays(-50),
                IsDeleted = false,
            };

            NinthBook = new Book()
            {
                Id = Guid.Parse("4C757EFD-2D1D-42A9-8460-88B9E1FFCC7D"),
                ISBN = "933-2-55-179400-1",
                Title = "Oliver Twist",
                YearPublished = DateOnly.ParseExact("1837", "yyyy", CultureInfo.InvariantCulture),
                Description = "Oliver Twist; or, The Parish Boy's Progress, is the second novel by English author Charles Dickens. It was originally published as a serial from 1837 to 1839 and as a three-volume book in 1838. The story follows the titular orphan, who, after being raised in a workhouse, escapes to London, where he meets a gang of juvenile pickpockets led by the elderly criminal Fagin, discovers the secrets of his parentage, and reconnects with his remaining family. Oliver Twist unromantically portrays the sordid lives of criminals and exposes the cruel treatment of the many orphans in London in the mid-19th century. The alternative title, The Parish Boy's Progress, alludes to Bunyan's The Pilgrim's Progress as well as the 18th-century caricature series by painter William Hogarth, A Rake's Progress and A Harlot's Progress.",
                Publisher = "Serialised 1837–1839; book form 1838",
                ImageFilePath = "img/BookCovers/f0e168fa-cc7c-43b3-979e-134a9b768303_Oliver-Twist-Image.jpg",
                AuthorId = Guid.Parse("1BAAD29E-BB6A-4424-95AE-9DCF01FA6712"),
                FilePath = "BookFiles/Books/3cc1eb86-3881-4da5-9d8c-014c4fae1680_Oliver-Twist.txt",
                CreatedOn = DateTime.UtcNow.AddDays(-10),
                IsDeleted = false,
            };

            // CATEGORIES
            FirstCategory = new Category()
            {
                Id = 1,
                Name = "Academic book",
            };

            SecondCategory = new Category()
            {
                Id = 2,
                Name = "Adventure stories",
            };

            ThirdCategory = new Category()
            {
                Id = 3,
                Name = "Clasics",
            };

            ForthCategory = new Category()
            {
                Id = 4,
                Name = "Mystery",
            };

            FifthCategory = new Category()
            {
                Id = 5,
                Name = "Roman",
            };

            SixthCategory = new Category()
            {
                Id = 6,
                Name = "Science fiction",
            };

            // EDITIONS
            FirstEdition = new Edition()
            {
                Id = Guid.Parse("1394B818-A8A6-474F-B123-5AD206C5F49D"),
                BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                Version = "Version 1",
                Publisher = "Hodder & Stoughton",
                EditionYear = DateOnly.ParseExact("2018", "yyyy", CultureInfo.InvariantCulture),
                FilePath = "BookFiles/Editions/8b4bdb27-41c8-4927-af72-576a7569af50_Brief-Answers-to-the-Big-Questions-Version-1.txt",
                IsDeleted = false,
            };

            SecondEdition = new Edition()
            {
                Id = Guid.Parse("C9F8D9F9-5C46-4C7B-9D7E-9F5F5A5F5F5F"),
                BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                Version = "Version 2",
                Publisher = "Hodder",
                EditionYear = DateOnly.ParseExact("2022", "yyyy", CultureInfo.InvariantCulture),
                FilePath = "BookFiles/Editions/697bfffb-5790-4604-8cb2-58054c20117b_Brief-Answers-to-the-Big-Questions-Version-2.txt",
                IsDeleted = false,
            };

            ThirdEdition = new Edition()
            {
                Id = Guid.Parse("A42D9106-0B58-4840-AD38-AD50DC0ACDC9"),
                BookId = Guid.Parse("42CBCCE4-349B-4D8C-A077-318A07BA74CC"),
                Version = "Version 2.0",
                Publisher = "Ace",
                EditionYear = DateOnly.ParseExact("2019", "yyyy", CultureInfo.InvariantCulture),
                FilePath = "BookFiles/Editions/6ff4f310-c7ca-4157-99c7-692a7fc2b6cd_Journey-to-the-Center-of-the-Earth-Version-2.txt",
                IsDeleted = false,
            };

            // RATINGS
            FirstRating = new Rating()
            {
                BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                UserId = Guid.Parse("89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153"),
                BookRating = 4.0m,
                Comment = "I thoroughly enjoyed reading this book. The storyline was captivating from start to finish, and the main character was incredibly well-developed. Their journey throughout the book kept me engaged, and I found myself rooting for them until the very end. The author did a fantastic job of creating a vivid and immersive world that pulled me in right from the first page. Overall, a truly exceptional read!",
            };

            SecondRating = new Rating()
            {
                BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                UserId = Guid.Parse("7F13235C-EAC9-4F60-AA69-BC8FC86FBD24"),
                BookRating = 4.5m,
                Comment = "This book exceeded all my expectations! The plot twists and turns kept me guessing until the very end, and I couldn't put it down. The main character was incredibly relatable, and I found myself emotionally invested in their journey. The author's writing style is both eloquent and engaging, making it easy to lose myself in the story. I would highly recommend this book to anyone looking for an unforgettable reading experience.",
            };

            ThirdRating = new Rating()
            {
                BookId = Guid.Parse("A6D5D2D7-A6FB-46EF-AA1D-9502A0EF1C50"),
                UserId = Guid.Parse("89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153"),
                BookRating = 4.0m,
                Comment = "This book left me speechless! From the moment I started reading, I was hooked. The main character's development throughout the story was nothing short of remarkable. Their strength, resilience, and vulnerability made them incredibly relatable, and I found myself rooting for them with every turn of the page. The author's descriptive prose transported me to another world, where I experienced every emotion alongside the characters. I laughed, I cried, and I was left in awe of the sheer brilliance of this book.",
            };

            // LENDED BOOKS
            FirstLendedBook = new LendedBook()
            {
                Id = Guid.Parse("19827894-2D91-4778-9338-2EEEA6D8FDCB"),
                LoanDate = DateTime.UtcNow.AddDays(-60),
                ReturnDate = DateTime.UtcNow.AddDays(-10),
                BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                UserId = Guid.Parse("7F13235C-EAC9-4F60-AA69-BC8FC86FBD24"),
            };

            SecondLendedBook = new LendedBook()
            {
                Id = Guid.Parse("A49C9AFC-6614-4BA9-BF33-04C9F0E4ACC5"),
                LoanDate = DateTime.UtcNow.AddDays(-60),
                BookId = Guid.Parse("50E9B56F-9BC1-4356-AC0C-E3D5945778BA"),
                UserId = Guid.Parse("7F13235C-EAC9-4F60-AA69-BC8FC86FBD24"),
            };

            ThirdLendedBook = new LendedBook()
            {
                Id = Guid.Parse("6DE0FDA4-77E3-4740-8C07-151F8CFCB211"),
                LoanDate = DateTime.UtcNow.AddDays(-60),
                BookId = Guid.Parse("4C757EFD-2D1D-42A9-8460-88B9E1FFCC7D"),
                UserId = Guid.Parse("7F13235C-EAC9-4F60-AA69-BC8FC86FBD24"),
            };

            ForthLendedBook = new LendedBook()
            {
                Id = Guid.Parse("6DE0F5A4-77E3-4540-8C07-151F8C5CB215"),
                LoanDate = DateTime.UtcNow.AddDays(-10),
                ReturnDate = null,
                BookId = Guid.Parse("4C757EFD-2D1D-42A9-8460-88B9E1FFCC7D"),
                UserId = Guid.Parse("89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153"),
            };

            FifthLendedBook = new LendedBook()
            {
                Id = Guid.Parse("AD4325A4-77E3-45B0-8C07-15AA8C5CB21A"),
                LoanDate = DateTime.UtcNow.AddDays(-50),
                ReturnDate = null,
                BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                UserId = Guid.Parse("BBBDF7F6-94E2-4BE8-A036-84AF63E028FC"),
            };

            // USERS
            FirstUser = new ApplicationUser()
            {
                Id = Guid.Parse("47C46DB7-E6F5-439C-88A8-CD144C55349A"),
                FirstName = "AdminFirst",
                LastName = "AdminLast",
                DateOfBirth = DateOnly.ParseExact("01.01.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Address = "Admin Address",
                Country = "Admin Country",
                City = "Admin City",
                UserName = "Admin-Username",
                PasswordHash = "pass.123",
                Email = "admin@elibrary.bg",
                PhoneNumber = "111222333",
                IsDeleted = false,
                SecurityStamp = "49CC835AFE4D41B5AB5DC8CB6886ACD0",
            };

            SecondUser = new ApplicationUser()
            {
                Id = Guid.Parse("7F13235C-EAC9-4F60-AA69-BC8FC86FBD24"),
                FirstName = "Dimitar",
                LastName = "Todorov",
                DateOfBirth = DateOnly.ParseExact("05.10.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Address = "Gurko 5",
                Country = "Bulgaria",
                City = "Sofia",
                UserName = "User-Username",
                MaxLoanedBooks = 5,
                PasswordHash = "pass.123",
                Email = "testUser@elibrary.bg",
                PhoneNumber = "444555666",
                IsDeleted = false,
                SecurityStamp = "6D9FB13AE0D145F496E624C798A5268E",
                CreatedOn = DateTime.UtcNow.AddDays(10),
            };

            ThirdUser = new ApplicationUser()
            {
                Id = Guid.Parse("89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153"),
                FirstName = "Marina",
                LastName = "Zdravkova",
                DateOfBirth = DateOnly.ParseExact("01.05.1985", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Address = "Hristo Botev 12",
                Country = "Bulgaria",
                City = "Haskovo",
                UserName = "Marina85",
                MaxLoanedBooks = 5,
                PasswordHash = "pass.123",
                Email = "marina85@abv.bg",
                PhoneNumber = "999666333",
                IsDeleted = false,
                SecurityStamp = "285DD83CB9D44020893880E27B178E15",
                CreatedOn = DateTime.UtcNow.AddDays(20),
            };

            ForthUser = new ApplicationUser()
            {
                Id = Guid.Parse("BBBDF7F6-94E2-4BE8-A036-84AF63E028FC"),
                FirstName = "Petko",
                LastName = "Georgiev",
                DateOfBirth = DateOnly.ParseExact("12.08.1978", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Address = "Pencho Vladigerov 15",
                Country = "Bulgaria",
                City = "Sofia",
                UserName = "PetkoBaby",
                MaxLoanedBooks = 5,
                PasswordHash = "pass.123",
                Email = "petkobaby@abv.bg",
                PhoneNumber = "778811547",
                IsDeleted = false,
                SecurityStamp = "618A8DDF778F4DDDB8A5B22CC61D1FAA",
                CreatedOn = DateTime.UtcNow.AddDays(30),
            };

            dbContext.Add(FirstAuthor);
            dbContext.Add(SecondAuthor);
            dbContext.Add(ThirdAuthor);
            dbContext.Add(ForthAuthor);
            dbContext.Add(FifthAuthor);

            dbContext.Add(FirstBook);
            dbContext.Add(SecondBook);
            dbContext.Add(ThirdBook);
            dbContext.Add(ForthBook);
            dbContext.Add(FifthBook);
            dbContext.Add(SixthBook);
            dbContext.Add(SeventhBook);
            dbContext.Add(EigthBook);
            dbContext.Add(NinthBook);

            dbContext.Add(FirstCategory);
            dbContext.Add(SecondCategory);
            dbContext.Add(ThirdCategory);
            dbContext.Add(ForthCategory);
            dbContext.Add(FifthCategory);
            dbContext.Add(SixthCategory);

            dbContext.Add(FirstEdition);
            dbContext.Add(SecondEdition);
            dbContext.Add(ThirdEdition);

            dbContext.Add(FirstRating);
            dbContext.Add(SecondRating);
            dbContext.Add(ThirdRating);

            dbContext.Add(FirstLendedBook);
            dbContext.Add(SecondLendedBook);
            dbContext.Add(ThirdLendedBook);
            dbContext.Add(ForthLendedBook);
            dbContext.Add(FifthLendedBook);

            dbContext.Add(FirstUser);
            dbContext.Add(SecondUser);
            dbContext.Add(ThirdUser);
            dbContext.Add(ForthUser);

            FirstAuthor.Books.Add(FirstBook);
            FirstAuthor.Books.Add(SecondBook);

            SecondAuthor.Books.Add(ThirdBook);
            SecondAuthor.Books.Add(ForthBook);

            ThirdAuthor.Books.Add(FifthBook);
            ThirdAuthor.Books.Add(SixthBook);

            ForthAuthor.Books.Add(SeventhBook);
            ForthAuthor.Books.Add(EigthBook);

            FifthAuthor.Books.Add(NinthBook);

            ThirdUser.LendedBooks.Add(ForthLendedBook);
            ForthUser.LendedBooks.Add(FifthLendedBook);

            dbContext.SaveChanges();
        }
    }
}
