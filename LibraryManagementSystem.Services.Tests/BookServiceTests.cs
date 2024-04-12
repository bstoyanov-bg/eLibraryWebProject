using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Author;
using LibraryManagementSystem.Web.ViewModels.Book;
using LibraryManagementSystem.Web.ViewModels.Category;
using LibraryManagementSystem.Web.ViewModels.Edition;
using LibraryManagementSystem.Web.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Globalization;
using static LibraryManagementSystem.Services.Tests.DatabaseSeeder;

namespace LibraryManagementSystem.Services.Tests
{
    public class BookServiceTests
    {
        private DbContextOptions<ELibraryDbContext> dbOptions;
        private ELibraryDbContext dbContext;

        private IBookService bookService;

        private Mock<ICategoryService> categoryServiceMock;
        private Mock<IAuthorService> authorServiceMock;
        private Mock<ILendedBookService> lendedBooksServiceMock;
        private Mock<IRatingService> ratingServiceMock;
        private Mock<IEditionService> editionServiceMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<ELibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "ELibraryTestDb")
                .Options;

            this.categoryServiceMock = new Mock<ICategoryService>();
            this.authorServiceMock = new Mock<IAuthorService>();
            this.lendedBooksServiceMock = new Mock<ILendedBookService>();
            this.ratingServiceMock = new Mock<IRatingService>();
            this.editionServiceMock = new Mock<IEditionService>();
        }

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new ELibraryDbContext(this.dbOptions);
            this.dbContext.Database.EnsureCreated();
            SeedDatabase(this.dbContext);

            this.bookService = new BookService(this.dbContext,
                categoryServiceMock.Object,
                authorServiceMock.Object,
                lendedBooksServiceMock.Object,
                new Lazy<IRatingService>(() => this.ratingServiceMock.Object),
                new Lazy<IEditionService>(() => this.editionServiceMock.Object));
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();

            this.categoryServiceMock.Reset();
            this.authorServiceMock.Reset();
            this.lendedBooksServiceMock.Reset();
            this.ratingServiceMock.Reset();
            this.editionServiceMock.Reset();
        }

        [Test]
        public async Task DeleteBookAsync_ValidBookId_SetsIsDeletedToTrue()
        {
            string bookId = FirstBook!.Id.ToString();

            await this.bookService.DeleteBookAsync(bookId);

            var deletedBook = await this.dbContext.Books.FindAsync(Guid.Parse(bookId));

            Assert.That(deletedBook!.IsDeleted, Is.True);
        }

        [Test]
        public async Task EditBookAsync_ValidBookId_UpdatesBookFields()
        {
            string bookId = SecondBook!.Id.ToString();
            var model = new BookFormModel
            {
                Title = "Edited Title",
                ISBN = "111-1-1111-1111-1",
                YearPublished = DateOnly.ParseExact("2000", "yyyy", CultureInfo.InvariantCulture),
                Description = "Edited Description",
                Publisher = "Edited Publisher",
                AuthorId = SecondAuthor!.Id.ToString(),
                CategoryId = 1,
            };

            var editedBook = await this.bookService.EditBookAsync(bookId, model);

            Assert.That(editedBook, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(editedBook.Title, Is.EqualTo(model.Title));
                Assert.That(editedBook.ISBN, Is.EqualTo(model.ISBN));
                Assert.That(editedBook.YearPublished, Is.EqualTo(model.YearPublished));
                Assert.That(editedBook.Description, Is.EqualTo(model.Description));
                Assert.That(editedBook.Publisher, Is.EqualTo(model.Publisher));
                Assert.That(editedBook.AuthorId.ToString(), Is.EqualTo(model.AuthorId));
            });

            // Assert category update
            var updatedBookCategories = await dbContext
                .BooksCategories
                .Where(bc => bc.BookId.ToString() == bookId)
                .ToListAsync();

            // Check if the updated book has any associated categories
            Assert.That(updatedBookCategories, Is.Not.Empty);
        }

        [Test]
        public async Task AddBookAsync_ValidModel_AddsBookAndCategory()
        {
            var model = new BookFormModel
            {
                ISBN = "111-1-1111-1111-1",
                Title = "New Book",
                Publisher = "New Publisher",
                YearPublished = DateOnly.ParseExact("2000", "yyyy", CultureInfo.InvariantCulture),
                Description = "New Description",
                ImageFilePath = "New Image File Path",
                AuthorId = "5992BC8E-87F8-41AF-9EFB-E2C354E4BD3B",
                CategoryId = 1,
            };

            var addedBook = await this.bookService.AddBookAsync(model);

            Assert.That(addedBook, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(addedBook.Title, Is.EqualTo(model.Title));
                Assert.That(addedBook.ISBN, Is.EqualTo(model.ISBN));
                Assert.That(addedBook.YearPublished, Is.EqualTo(model.YearPublished));
                Assert.That(addedBook.Description, Is.EqualTo(model.Description));
                Assert.That(addedBook.Publisher, Is.EqualTo(model.Publisher));
                Assert.That(addedBook.AuthorId.ToString().ToUpper(), Is.EqualTo(model.AuthorId));
                Assert.That(addedBook.ImageFilePath, Is.EqualTo(model.ImageFilePath));
            });

            var addedBookCategory = await dbContext.BooksCategories.FirstOrDefaultAsync(bc => bc.BookId == addedBook.Id);
            Assert.That(addedBookCategory, Is.Not.Null);
            Assert.That(addedBookCategory.CategoryId, Is.EqualTo(model.CategoryId));
        }

        [Test]
        public async Task GetBookByIdAsync_ValidBookId_ReturnsBook()
        {
            string validBookId = FirstBook!.Id.ToString();

            var book = await this.bookService.GetBookByIdAsync(validBookId);

            Assert.That(book, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(book!.Id.ToString(), Is.EqualTo(validBookId));
                Assert.That(book.IsDeleted, Is.False);
            });
        }

        [Test]
        public async Task GetBookByIdAsync_InvalidBookId_ReturnsNull()
        {
            string invalidBookId = "invalid_id";

            var book = await this.bookService.GetBookByIdAsync(invalidBookId);

            Assert.That(book, Is.Null);
        }

        [Test]
        public async Task GetCreateNewBookModelAsync_ReturnsPopulatedModel()
        {
            var expectedAuthors = new List<AuthorsSelectForBookFormModel>
            {
                new AuthorsSelectForBookFormModel { Id = FirstAuthor!.Id.ToString(), Name = FirstAuthor!.FirstName + " " + FirstAuthor!.LastName, Nationality = FirstAuthor!.Nationality },
                new AuthorsSelectForBookFormModel { Id = SecondAuthor!.Id.ToString(), Name = SecondAuthor!.FirstName + " " + SecondAuthor!.LastName, Nationality = SecondAuthor!.Nationality },
            };

            var expectedCategories = new List<AllCategoriesViewModel>
            {
                new AllCategoriesViewModel { Id = FirstCategory!.Id, Name = FirstCategory.Name },
                new AllCategoriesViewModel { Id = SecondCategory!.Id, Name = SecondCategory.Name },
            };

            this.authorServiceMock.Setup(m => m.GetAllAuthorsForListAsync()).ReturnsAsync(expectedAuthors);

            this.categoryServiceMock.Setup(m => m.GetAllCategoriesAsync()).ReturnsAsync(expectedCategories);

            var model = await this.bookService.GetCreateNewBookModelAsync();

            Assert.That(model, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(model.Authors, Is.EqualTo(expectedAuthors));
                Assert.That(model.Categories, Is.EqualTo(expectedCategories));
            });
        }

        [Test]
        public async Task GetBookForEditByIdAsync_WithValidBookId_ReturnsBookFormModel()
        {
            string bookId = FirstBook!.Id.ToString();
            var expectedAuthors = new List<AuthorsSelectForBookFormModel>
            {
                new AuthorsSelectForBookFormModel { Id = FirstAuthor!.Id.ToString(), Name = FirstAuthor!.FirstName + " " + FirstAuthor!.LastName, Nationality = FirstAuthor!.Nationality },
                new AuthorsSelectForBookFormModel { Id = SecondAuthor!.Id.ToString(), Name = SecondAuthor!.FirstName + " " + SecondAuthor!.LastName, Nationality = SecondAuthor!.Nationality },
            };

            var expectedCategories = new List<AllCategoriesViewModel>
            {
                new AllCategoriesViewModel { Id = FirstCategory!.Id, Name = FirstCategory.Name },
                new AllCategoriesViewModel { Id = SecondCategory!.Id, Name = SecondCategory.Name },
            };

            var expectedBook = new Book
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

            authorServiceMock.Setup(m => m.GetAllAuthorsForListAsync()).ReturnsAsync(expectedAuthors);
            categoryServiceMock.Setup(m => m.GetAllCategoriesAsync()).ReturnsAsync(expectedCategories);
            categoryServiceMock.Setup(m => m.GetCategoryIdByBookIdAsync(bookId)).ReturnsAsync(2);

            var result = await bookService.GetBookForEditByIdAsync(bookId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(expectedBook.Id.ToString()));
            Assert.That(result.Title, Is.EqualTo(expectedBook.Title));
            Assert.That(result.ISBN, Is.EqualTo(expectedBook.ISBN));
            Assert.That(result.YearPublished, Is.EqualTo(expectedBook.YearPublished));
            Assert.That(result.Description, Is.EqualTo(expectedBook.Description));
            Assert.That(result.Publisher, Is.EqualTo(expectedBook.Publisher));
            Assert.That(result.AuthorId, Is.EqualTo(expectedBook.AuthorId.ToString()));
            Assert.That(result.ImageFilePath, Is.EqualTo(expectedBook.ImageFilePath));
            Assert.That(result.FilePath, Is.EqualTo(expectedBook.FilePath));
            Assert.That(result.CategoryId, Is.EqualTo(2));
            Assert.That(result.Authors, Is.EqualTo(expectedAuthors));
            Assert.That(result.Categories, Is.EqualTo(expectedCategories));
        }

        [Test]
        public async Task GetBookDetailsForUserAsync_ReturnsBookDetails()
        {
            string bookId = FirstBook!.Id.ToString();
            var expectedBook = FirstBook;

            var expectedEditions = new List<EditionsForBookDetailsViewModel>();
            var expectedCategoryName = "Academic book";
            decimal? expectedBookRating = 4.5m;
            var expectedPeopleReading = 1;

            editionServiceMock.Setup(x => x.GetEditionsForBookDetailsViewModelAsync(bookId))
                               .ReturnsAsync(expectedEditions);

            categoryServiceMock.Setup(x => x.GetCategoryNameByBookIdAsync(bookId))
                                .ReturnsAsync(expectedCategoryName);

            ratingServiceMock.Setup(x => x.GetAverageRatingForBookAsync(bookId))
                              .ReturnsAsync(expectedBookRating);

            lendedBooksServiceMock.Setup(x => x.GetCountOfPeopleReadingTheBookAsync(bookId))
                                   .ReturnsAsync(expectedPeopleReading);

            var result = await bookService.GetBookDetailsForUserAsync(bookId);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(expectedBook.Id.ToString()));
                Assert.That(result.Title, Is.EqualTo(expectedBook.Title));
                Assert.That(result.ISBN, Is.EqualTo(expectedBook.ISBN));
                Assert.That(result.YearPublished, Is.EqualTo(expectedBook.YearPublished));
                Assert.That(result.Description, Is.EqualTo(expectedBook.Description));
                Assert.That(result.Publisher, Is.EqualTo(expectedBook.Publisher));
                Assert.That(result.ImageFilePath, Is.EqualTo(expectedBook.ImageFilePath));
                Assert.That(result.Editions, Is.EqualTo(expectedEditions));
                Assert.That(result.CategoryName, Is.EqualTo(expectedCategoryName));
                Assert.That(result.Rating, Is.EqualTo(expectedBookRating));
                Assert.That(result.PeopleReading, Is.EqualTo(expectedPeopleReading));
            });
        }

        // STRANGE PROBLEM WITH THE DB OR THE QUERY ???
        //[Test]
        //public async Task GetAllBooksFilteredAndPagedAsync_ReturnsExpectedResult()
        //{
        //    // Arrange
        //    var queryModel = new AllBooksQueryModel()
        //    {
        //        Category = null,
        //        SearchString = "Time Shelter",
        //        BookSorting = BookSorting.Newest,
        //        CurrentPage = 1,
        //        BooksPerPage = 10,
        //    };

        //    // Expected result based on the test data
        //    var expectedTotalBooksCount = 1; // Adjust based on your test data

        //    // Act
        //    var result = await bookService.GetAllBooksFilteredAndPagedAsync(queryModel);
        //    int actualCount = await this.dbContext.Books.CountAsync(c => c.Title.Contains(queryModel.SearchString));

        //    // Assert
        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result.Books.Count(), Is.EqualTo(actualCount));
        //    Assert.That(result.TotalBooksCount, Is.EqualTo(expectedTotalBooksCount));
        //}


        [Test]
        public async Task BookExistByIdAsync_BookExists_ReturnsTrue()
        {
            Book? book = FirstBook;

            bool bookExists = await this.bookService.BookExistByIdAsync(book!.Id.ToString());

            Assert.That(bookExists, Is.True);
        }

        [Test]
        public async Task BookExistByIdAsync_BookDoesNotExist_ReturnsFalse()
        {
            var bookId = Guid.NewGuid().ToString();

            bool bookExists = await this.bookService.BookExistByIdAsync(bookId);

            Assert.That(bookExists, Is.False);
        }

        [Test]
        public async Task BookExistByTitleAndAuthorIdAsync_BookExists_ReturnsTrue()
        {
            string title = FirstBook!.Title;
            string authorId = FirstBook.AuthorId.ToString();

            bool bookExists = await this.bookService.BookExistByTitleAndAuthorIdAsync(title, authorId);

            Assert.That(bookExists, Is.True);
        }

        [Test]
        public async Task BookExistByTitleAndAuthorIdAsync_BookDoesNotExist_ReturnsFalse()
        {
            string title = "Non-existing Book Title";
            string authorId = Guid.NewGuid().ToString();

            bool bookExists = await this.bookService.BookExistByTitleAndAuthorIdAsync(title, authorId);

            Assert.That(bookExists, Is.False);
        }

        [Test]
        public async Task BookExistByISBNAsync_BookExists_ReturnsTrue()
        {
            string existingISBN = ThirdBook!.ISBN!;

            bool bookExists = await this.bookService.BookExistByISBNAsync(existingISBN);

            Assert.That(bookExists, Is.True);
        }

        [Test]
        public async Task BookExistByISBNAsync_BookDoesNotExist_ReturnsFalse()
        {
            string nonExistingISBN = "000-0-00-000000-0";

            bool bookExists = await this.bookService.BookExistByISBNAsync(nonExistingISBN);

            Assert.That(bookExists, Is.False);
        }

        [Test]
        public async Task DoesBookHaveUploadedFileAsync_BookHasUploadedFile_ReturnsTrue()
        {
            string bookId = SecondBook!.Id.ToString();

            bool hasUploadedFile = await this.bookService.DoesBookHaveUploadedFileAsync(bookId);

            Assert.That(hasUploadedFile, Is.True);
        }

        [Test]
        public async Task DoesBookHaveUploadedFileAsync_BookDoesNotHaveUploadedFile_ReturnsFalse()
        {
            string bookId = Guid.NewGuid().ToString();

            bool hasUploadedFile = await this.bookService.DoesBookHaveUploadedFileAsync(bookId);

            Assert.That(hasUploadedFile, Is.False);
        }

        [Test]
        public async Task GetCountOfActiveBooksAsync_ReturnsCountOfActiveBooks()
        {
            List<Book> activeBooks = new List<Book>
            {
                FirstBook!,
                SecondBook!,
                ThirdBook!,
                ForthBook!,
                FifthBook!,
                SixthBook!,
                SeventhBook!,
                NinthBook!,
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
                }
            };

            int activeBooksCount = await this.bookService.GetCountOfActiveBooksAsync();

            Assert.That(activeBooksCount, Is.EqualTo(activeBooks.Count));
        }

        [Test]
        public async Task GetCountOfDeletedBooksAsync_ReturnsCountOfDeletedBooks()
        {
            List<Book> deletedBooks = new List<Book>
            {
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
                    IsDeleted = true,
                },
            };

            await this.dbContext.Books.AddRangeAsync(deletedBooks);
            await this.dbContext.SaveChangesAsync();

            int deletedBooksCount = await this.bookService.GetCountOfDeletedBooksAsync();

            Assert.That(deletedBooksCount, Is.EqualTo(deletedBooks.Count));
        }

        [Test]
        public async Task LastNineBooksAsync_ReturnsLastNineAddedBooks()
        {
            var books = new List<IndexViewModel>
            {
                new IndexViewModel { Id = ForthBook!.Id.ToString().ToLower(), Description = ForthBook.Description, Title = ForthBook.Title, Author = $"{ForthBook.Author.FirstName} {ForthBook.Author.LastName}", ImageFilePath = ForthBook.ImageFilePath! },
                new IndexViewModel { Id = FifthBook!.Id.ToString().ToLower(), Description = FifthBook.Description, Title = FifthBook.Title, Author = $"{FifthBook.Author.FirstName} {FifthBook.Author.LastName}", ImageFilePath = FifthBook.ImageFilePath! },
                new IndexViewModel { Id = FirstBook!.Id.ToString().ToLower(), Description = FirstBook.Description, Title = FirstBook.Title, Author = $"{FirstBook.Author.FirstName} {FirstBook.Author.LastName}", ImageFilePath = FirstBook.ImageFilePath! },
                new IndexViewModel { Id = SixthBook!.Id.ToString().ToLower(), Description = SixthBook.Description, Title = SixthBook.Title, Author = $"{SixthBook.Author.FirstName} {SixthBook.Author.LastName}", ImageFilePath = SixthBook.ImageFilePath! },
                new IndexViewModel { Id = SeventhBook!.Id.ToString().ToLower(), Description = SeventhBook.Description, Title = SeventhBook.Title, Author = $"{SeventhBook.Author.FirstName} {SeventhBook.Author.LastName}", ImageFilePath = SeventhBook.ImageFilePath! },
                new IndexViewModel { Id = NinthBook!.Id.ToString().ToLower(), Description = NinthBook.Description, Title = NinthBook.Title, Author = $"{NinthBook.Author.FirstName} {NinthBook.Author.LastName}", ImageFilePath = NinthBook.ImageFilePath! },
                new IndexViewModel { Id = SecondBook!.Id.ToString().ToLower(), Description = SecondBook.Description, Title = SecondBook.Title, Author = $"{SecondBook.Author.FirstName} {SecondBook.Author.LastName}", ImageFilePath = SecondBook.ImageFilePath! },
                new IndexViewModel { Id = EigthBook!.Id.ToString().ToLower(), Description = EigthBook.Description, Title = EigthBook.Title, Author = $"{EigthBook.Author.FirstName} {EigthBook.Author.LastName}", ImageFilePath = EigthBook.ImageFilePath! },
                new IndexViewModel { Id = ThirdBook!.Id.ToString().ToLower(), Description = ThirdBook.Description, Title = ThirdBook.Title, Author = $"{ThirdBook.Author.FirstName} {ThirdBook.Author.LastName}", ImageFilePath = ThirdBook.ImageFilePath! },
            };

            IEnumerable<IndexViewModel> lastNineBooks = await this.bookService.LastNineBooksAsync();

            Assert.That(lastNineBooks.Count(), Is.EqualTo(books.Count)); // Ensure only 9 books are returned

            // Verify if the returned books are the last 9 added non-deleted books in the database
            for (int i = 0; i < lastNineBooks.Count(); i++)
            {
                var expectedBook = books[i];
                var actualBook = lastNineBooks.ElementAt(i);

                Assert.That(actualBook.Id, Is.EqualTo(expectedBook.Id));
                Assert.That(actualBook.Title, Is.EqualTo(expectedBook.Title));
                Assert.That(actualBook.Description, Is.EqualTo(expectedBook.Description));
                Assert.That(actualBook.Author, Is.EqualTo(expectedBook.Author));
                Assert.That(actualBook.ImageFilePath, Is.EqualTo(expectedBook.ImageFilePath ?? string.Empty));
            }
        }

        [Test]
        public async Task GetAllBooksForListAsync_ReturnsAllActiveBooks()
        {
            var expectedBooks = new List<BookSelectForEditionFormModel>
            {
                new BookSelectForEditionFormModel { Id = FirstBook!.Id.ToString().ToLower(), Title = FirstBook.Title, AuthorName = $"{FirstBook.Author.FirstName} {FirstBook.Author.LastName}", YearPublished = FirstBook.YearPublished.ToString() ?? string.Empty, Publisher = FirstBook.Publisher ?? string.Empty },
                new BookSelectForEditionFormModel { Id = SecondBook!.Id.ToString().ToLower(), Title = SecondBook.Title, AuthorName = $"{SecondBook.Author.FirstName} {SecondBook.Author.LastName}", YearPublished = SecondBook.YearPublished.ToString() ?? string.Empty, Publisher = SecondBook.Publisher ?? string.Empty },
                new BookSelectForEditionFormModel { Id = ThirdBook!.Id.ToString().ToLower(), Title = ThirdBook.Title, AuthorName = $"{ThirdBook.Author.FirstName} {ThirdBook.Author.LastName}", YearPublished = ThirdBook.YearPublished.ToString() ?? string.Empty, Publisher = ThirdBook.Publisher ?? string.Empty },
                new BookSelectForEditionFormModel { Id = ForthBook!.Id.ToString().ToLower(), Title = ForthBook.Title, AuthorName = $"{ForthBook.Author.FirstName} {ForthBook.Author.LastName}", YearPublished = ForthBook.YearPublished.ToString() ?? string.Empty, Publisher = ForthBook.Publisher ?? string.Empty },
                new BookSelectForEditionFormModel { Id = FifthBook!.Id.ToString().ToLower(), Title = FifthBook.Title, AuthorName = $"{FifthBook.Author.FirstName} {FifthBook.Author.LastName}", YearPublished = FifthBook.YearPublished.ToString() ?? string.Empty, Publisher = FifthBook.Publisher ?? string.Empty },
                new BookSelectForEditionFormModel { Id = SixthBook!.Id.ToString().ToLower(), Title = SixthBook.Title, AuthorName = $"{SixthBook.Author.FirstName} {SixthBook.Author.LastName}", YearPublished = SixthBook.YearPublished.ToString() ?? string.Empty, Publisher = SixthBook.Publisher ?? string.Empty },
                new BookSelectForEditionFormModel { Id = SeventhBook!.Id.ToString().ToLower(), Title = SeventhBook.Title, AuthorName = $"{SeventhBook.Author.FirstName} {SeventhBook.Author.LastName}", YearPublished = SeventhBook.YearPublished.ToString() ?? string.Empty, Publisher = SeventhBook.Publisher ?? string.Empty },
                new BookSelectForEditionFormModel { Id = EigthBook!.Id.ToString().ToLower(), Title = EigthBook.Title, AuthorName = $"{EigthBook.Author.FirstName} {EigthBook.Author.LastName}", YearPublished = EigthBook.YearPublished.ToString() ?? string.Empty, Publisher = EigthBook.Publisher ?? string.Empty },
                new BookSelectForEditionFormModel { Id = NinthBook!.Id.ToString().ToLower(), Title = NinthBook.Title, AuthorName = $"{NinthBook.Author.FirstName} {NinthBook.Author.LastName}", YearPublished = NinthBook.YearPublished.ToString() ?? string.Empty, Publisher = NinthBook.Publisher ?? string.Empty },
            };

            var actualBooks = await this.bookService.GetAllBooksForListAsync();

            Assert.That(actualBooks.Count(), Is.EqualTo(expectedBooks.Count));

            for (int i = 0; i < expectedBooks.Count; i++)
            {
                var expectedBook = expectedBooks[i];
                var actualBook = actualBooks.ElementAt(i);

                Assert.Multiple(() =>
                {
                    // Assert individual properties
                    Assert.That(actualBook.Id, Is.EqualTo(expectedBook.Id), $"Id mismatch at index {i}");
                    Assert.That(actualBook.Title, Is.EqualTo(expectedBook.Title), $"Title mismatch for book with Id {expectedBook.Id}");
                    Assert.That(actualBook.AuthorName, Is.EqualTo(expectedBook.AuthorName), $"AuthorName mismatch for book with Id {expectedBook.Id}");
                    Assert.That(actualBook.YearPublished, Is.EqualTo(expectedBook.YearPublished), $"YearPublished mismatch for book with Id {expectedBook.Id}");
                    Assert.That(actualBook.Publisher, Is.EqualTo(expectedBook.Publisher), $"Publisher mismatch for book with Id {expectedBook.Id}");
                });
            }
        }

        [Test]
        public async Task GetBooksForAuthorDetailsAsync_ReturnsBooksForAuthor()
        {
            string authorId = FirstAuthor!.Id.ToString();

            var authorBooks = new List<BooksForAuthorDetailsViewModel>
            {
                new BooksForAuthorDetailsViewModel { Id = FirstBook!.Id.ToString().ToLower(), Title = FirstBook.Title, ISBN = FirstBook.ISBN ?? string.Empty, Description = FirstBook.Description, YearPublished = FirstBook.YearPublished ?? default, Publisher = FirstBook.Publisher ?? null, ImageFilePath = FirstBook.ImageFilePath },
                new BooksForAuthorDetailsViewModel { Id = SecondBook!.Id.ToString().ToLower(), Title = SecondBook.Title, ISBN = SecondBook.ISBN ?? string.Empty, Description = SecondBook.Description, YearPublished = SecondBook.YearPublished ?? default, Publisher = SecondBook.Publisher ?? null, ImageFilePath = SecondBook.ImageFilePath },
            };

            var result = await bookService.GetBooksForAuthorDetailsAsync(authorId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(authorBooks.Count));

            foreach (var expectedBook in authorBooks)
            {
                var actualBook = result.FirstOrDefault(b => b.Id == expectedBook.Id.ToString());
                Assert.That(actualBook, Is.Not.Null); // Ensure book exists in the result

                Assert.Multiple(() =>
                {
                    // Assert individual properties
                    Assert.That(actualBook.Title, Is.EqualTo(expectedBook.Title));
                    Assert.That(actualBook.ISBN, Is.EqualTo(expectedBook.ISBN));
                    Assert.That(actualBook.YearPublished, Is.EqualTo(expectedBook.YearPublished));
                    Assert.That(actualBook.Description, Is.EqualTo(expectedBook.Description));
                    Assert.That(actualBook.Publisher, Is.EqualTo(expectedBook.Publisher));
                    Assert.That(actualBook.ImageFilePath, Is.EqualTo(expectedBook.ImageFilePath));
                });
            }
        }
    }
}
