using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Data.Seeding.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data.Seeding
{
    public class RatingSeeder : ISeeder
    {
        public async Task SeedAsync(ELibraryDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Ratings.AnyAsync())
            {
                return;
            }

            IEnumerable<Rating> ratings = new HashSet<Rating>
            {
                new Rating
                {
                    BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                    UserId = Guid.Parse("89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153"),
                    BookRating = 4.0m,
                    Comment = "I thoroughly enjoyed reading this book. The storyline was captivating from start to finish, and the main character was incredibly well-developed. Their journey throughout the book kept me engaged, and I found myself rooting for them until the very end. The author did a fantastic job of creating a vivid and immersive world that pulled me in right from the first page. Overall, a truly exceptional read!",
                },
                new Rating
                {
                    BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                    UserId = Guid.Parse("7F13235C-EAC9-4F60-AA69-BC8FC86FBD24"),
                    BookRating = 4.5m,
                    Comment = "This book exceeded all my expectations! The plot twists and turns kept me guessing until the very end, and I couldn't put it down. The main character was incredibly relatable, and I found myself emotionally invested in their journey. The author's writing style is both eloquent and engaging, making it easy to lose myself in the story. I would highly recommend this book to anyone looking for an unforgettable reading experience.",
                },
                new Rating
                {
                    BookId = Guid.Parse("50E9B56F-9BC1-4356-AC0C-E3D5945778BA"),
                    UserId = Guid.Parse("89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153"),
                    BookRating = 4.0m,
                    Comment = "What a masterpiece! This book is a true work of art. The main character's depth and complexity added layers to the story, and I found myself connecting with them on a profound level. The author's attention to detail is impeccable, painting a vivid picture of the world they've created. From the gripping plot to the richly developed characters, every aspect of this book is masterfully crafted. I couldn't put it down and was left longing for more long after I finished the final page.",
                },
                new Rating
                {
                    BookId = Guid.Parse("DDDED6BD-AAB9-4503-B285-AA2DE7FF7BC3"),
                    UserId = Guid.Parse("7F13235C-EAC9-4F60-AA69-BC8FC86FBD24"),
                    BookRating = 3.5m,
                    Comment = "I was completely swept away by this book! The main character's journey was not only compelling but also deeply moving. Their struggles and triumphs resonated with me on a personal level, and I found myself reflecting on their experiences long after I finished reading. The author's ability to craft such a rich and immersive story is truly remarkable. Every page was filled with emotion, tension, and excitement, making it impossible to put down. I wholeheartedly recommend this book to anyone seeking a poignant and unforgettable reading experience.",
                },
                new Rating
                {
                    BookId = Guid.Parse("A6D5D2D7-A6FB-46EF-AA1D-9502A0EF1C50"),
                    UserId = Guid.Parse("89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153"),
                    BookRating = 4.0m,
                    Comment = "This book left me speechless! From the moment I started reading, I was hooked. The main character's development throughout the story was nothing short of remarkable. Their strength, resilience, and vulnerability made them incredibly relatable, and I found myself rooting for them with every turn of the page. The author's descriptive prose transported me to another world, where I experienced every emotion alongside the characters. I laughed, I cried, and I was left in awe of the sheer brilliance of this book.",
                },
                new Rating
                {
                    BookId = Guid.Parse("42CBCCE4-349B-4D8C-A077-318A07BA74CC"),
                    UserId = Guid.Parse("BBBDF7F6-94E2-4BE8-A036-84AF63E028FC"),
                    BookRating = 5.0m,
                    Comment = "An absolute gem of a book! The main character's journey was filled with twists and turns that kept me on the edge of my seat until the very end. Their growth and transformation throughout the story were both inspiring and heartwarming. The author's vivid descriptions painted a picture so clear, it felt as though I was living alongside the characters. This book touched my heart in ways I never expected, and I know it's one I'll revisit time and time again. If you're looking for a story that will stay with you long after you've finished reading, look no further.",
                },
            };

            await dbContext.Ratings.AddRangeAsync(ratings);
            await dbContext.SaveChangesAsync();
        }
    }
}
