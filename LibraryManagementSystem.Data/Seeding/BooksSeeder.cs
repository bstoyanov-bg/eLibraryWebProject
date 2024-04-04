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
                    ImageFilePath = "img/BookCovers/3a740213-ae68-43ea-8d78-79d0656581a7_Brief-Answers-to-the-Big-Questions-Image.png",
                    AuthorId = Guid.Parse("3CF69D33-43D6-4AD9-8569-0C2A897A66C0"),
                    FilePath = "BookFiles/Books/f4f985e7-0431-428f-91ab-e7d585a192cd_Brief-Answers-to-the-Big-Questions.txt",
                    CreatedOn = DateTime.UtcNow.AddDays(10),
                    IsDeleted = false,
                },
                new Book
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
                },
                new Book
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
                },
                new Book
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
                },
                new Book
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
                },
                new Book
                {
                    Id = Guid.Parse("8C6D196A-1E96-4DA0-9B65-91CD7736E13E"),
                    ISBN = "813-2-51-1788300-8",
                    Title = "The Old Curiosity Shop",
                    YearPublished = DateOnly.ParseExact("1841", "yyyy", CultureInfo.InvariantCulture),
                    Description = "The Old Curiosity Shop is one of two novels (the other being Barnaby Rudge) which Charles Dickens published along with short stories in his weekly serial Master Humphrey's Clock, from 1840 to 1841. It was so popular that New York readers stormed the wharf when the ship bearing the final instalment arrived in 1841. The Old Curiosity Shop was printed in book form in 1841. Queen Victoria read the novel that year and found it very interesting and cleverly written. The plot follows the journey of Nell Trent and her grandfather, both residents of The Old Curiosity Shop in London, whose lives are thrown into disarray and destitution due to the machinations of an evil moneylender.",
                    ImageFilePath = "img/BookCovers/e90d8731-5e98-40fa-b332-1fe0a7449075_The-Old-Curiosity-Shop-Image.jpg",
                    AuthorId = Guid.Parse("1BAAD29E-BB6A-4424-95AE-9DCF01FA6712"),
                    FilePath = "BookFiles/Books/a6c767ec-b245-4713-a080-f41f5a84e5d7_The-Old-Curiosity-Shop.txt",
                    CreatedOn = DateTime.UtcNow.AddDays(-20),
                    IsDeleted = false,
                },
                new Book
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
                },
                new Book
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
                },
                new Book
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
                },
                new Book
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
                },
                new Book
                {
                    Id = Guid.Parse("6232A651-266B-45BC-B652-9696553914B7"),
                    ISBN = "581-1-74-1824874-2",
                    Title = "Shantaram",
                    YearPublished = DateOnly.ParseExact("2003", "yyyy", CultureInfo.InvariantCulture),
                    Description = "It took me a long time and most of the world to learn what I know about love and fate and the choices we make, but the heart of it came to me in an instant, while I was chained to a wall and being tortured. So begins this epic, mesmerizing first novel set in the underworld of contemporary Bombay. Shantaram is narrated by Lin, an escaped convict with a false passport who flees maximum security prison in Australia for the teeming streets of a city where he can disappear. Accompanied by his guide and faithful friend, Prabaker, the two enter Bombay's hidden society of beggars and gangsters, prostitutes and holy men, soldiers and actors, and Indians and exiles from other countries, who seek in this remarkable place what they cannot find elsewhere.",
                    Publisher = "Scribe",
                    ImageFilePath = "img/BookCovers/10e920e1-2abe-4249-91b3-76ba89d7d6ad_Shantaram-Image.jpg",
                    AuthorId = Guid.Parse("1B019738-D4EA-4D18-84D4-F0C2A7F5AAAF"),
                    FilePath = "BookFiles/Books/02b3f989-6baf-4e20-8713-922eb122ff26_Shantaram.txt",
                    CreatedOn = DateTime.UtcNow.AddDays(5),
                    IsDeleted = false,
                },
                new Book
                {
                    Id = Guid.Parse("A6D5D2D7-A6FB-46EF-AA1D-9502A0EF1C50"),
                    ISBN = "364-1-87-1825743-8",
                    Title = "The Mountain Shadow",
                    YearPublished = DateOnly.ParseExact("2015", "yyyy", CultureInfo.InvariantCulture),
                    Description = "A sequel to SHANTARAM but equally a standalone novel, The Mountain Shadow follows Lin on further adventures in shadowy worlds and cultures. It is a novel about seeking identity, love, meaning, purpose, home, even the secret of life...As the story begins, Lin has found happiness and love, but when he gets a call that a friend is in danger, he has no choice but to go to his aid, even though he knows that leaving this paradise puts everything at risk, including himself and his lover. When he arrives to fulfil his obligation, he enters a room with eight men: each will play a significant role in the story that follows. One will become a friend, one an enemy, one will try to kill Lin, one will be killed by another... Some characters appeared in Shantaram, others are introduced for the first time, including Navida Der, a half-Irish, half-Indian detective, and Edras, a philosopher with fundamental beliefs. Gregory David Roberts is an extraordinarily gifted writer whose stories are richly rewarding on many levels. Like Shantaram, The Mountain Shadow will be a compelling adventure story with a profound message at its heart.",
                    Publisher = "Grove Press",
                    ImageFilePath = "img/BookCovers/6603260d-bd1a-4bab-bc25-13abb33a0062_The-Mountain-Shadow-Image.jpg",
                    AuthorId = Guid.Parse("1B019738-D4EA-4D18-84D4-F0C2A7F5AAAF"),
                    FilePath = "BookFiles/Books/2972e428-9b13-41c8-8c31-fd60fe752e61_The-Mountain-Shadow.txt",
                    CreatedOn = DateTime.UtcNow.AddDays(-100),
                    IsDeleted = false,
                },
            };

            await dbContext.Books.AddRangeAsync(books);
            await dbContext.SaveChangesAsync();
        }
    }
}
