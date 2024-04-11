using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Author;
using LibraryManagementSystem.Web.ViewModels.Category;
using LibraryManagementSystem.Web.ViewModels.Edition;
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
        private Mock<ILendedBooksService> lendedBooksServiceMock;
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
            this.lendedBooksServiceMock = new Mock<ILendedBooksService>();
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

        //[Test]
        //public async Task EditBookAsync_ValidBookId_UpdatesBookFields()
        //{
        //    string bookId = SecondBook!.Id.ToString();
        //    var model = new BookFormModel
        //    {
        //        Title = "Edited Title",
        //        ISBN = "111-1-1111-1111-1",
        //        YearPublished = DateOnly.ParseExact("2000", "yyyy", CultureInfo.InvariantCulture),
        //        Description = "Edited Description",
        //        Publisher = "Edited Publisher",
        //        ImageFilePath = "Edited Image File Path",
        //        FilePath = "Edited File Path",
        //        AuthorId = SecondAuthor!.Id.ToString(),
        //        CategoryId = 1,
        //    };

        //    // Act
        //    var editedBook = await this.bookService.EditBookAsync(bookId, model);

        //    // Assert
        //    Assert.That(editedBook, Is.Not.Null);
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(editedBook.Title, Is.EqualTo(model.Title));
        //        Assert.That(editedBook.ISBN, Is.EqualTo(model.ISBN));
        //        Assert.That(editedBook.YearPublished, Is.EqualTo(model.YearPublished));
        //        Assert.That(editedBook.Description, Is.EqualTo(model.Description));
        //        Assert.That(editedBook.Publisher, Is.EqualTo(model.Publisher));
        //        Assert.That(editedBook.ImageFilePath, Is.EqualTo(model.ImageFilePath));
        //        Assert.That(editedBook.FilePath, Is.EqualTo(model.FilePath));
        //        Assert.That(editedBook.AuthorId.ToString(), Is.EqualTo(model.AuthorId));
        //    });

        //    // Assert category update
        //    var updatedBookCategories = await dbContext.BooksCategories
        //        .Where(bc => bc.BookId.ToString() == bookId)
        //        .ToListAsync();

        //    // Check if the updated book has any associated categories
        //    Assert.That(updatedBookCategories, Is.Not.Empty);
        //}

        //[Test]
        //public async Task AddBookAsync_ValidModel_AddsBookAndCategory()
        //{
        //    // Arrange
        //    var model = new BookFormModel
        //    {
        //        ISBN = "111-1-1111-1111-1",
        //        Title = "New Book",
        //        Publisher = "New Publisher",
        //        YearPublished = DateOnly.ParseExact("2000", "yyyy", CultureInfo.InvariantCulture),
        //        Description = "New Description",
        //        ImageFilePath = "New Image File Path",
        //        AuthorId = "5992BC8E-87F8-41AF-9EFB-E2C354E4BD3B",
        //        CategoryId = 1,
        //    };

        //    // Act
        //    var addedBook = await this.bookService.AddBookAsync(model);

        //    // Assert
        //    Assert.That(addedBook, Is.Not.Null);
        //    Assert.That(addedBook.Title, Is.EqualTo(model.Title));
        //    Assert.That(addedBook.ISBN, Is.EqualTo(model.ISBN));
        //    Assert.That(addedBook.YearPublished, Is.EqualTo(model.YearPublished));
        //    Assert.That(addedBook.Description, Is.EqualTo(model.Description));
        //    Assert.That(addedBook.Publisher, Is.EqualTo(model.Publisher));
        //    Assert.That(addedBook.AuthorId.ToString(), Is.EqualTo(model.AuthorId));
        //    Assert.That(addedBook.ImageFilePath, Is.EqualTo(model.ImageFilePath));

        //    // Verify book category association
        //    var addedBookCategory = await dbContext.BooksCategories.FirstOrDefaultAsync(bc => bc.BookId == addedBook.Id);
        //    Assert.That(addedBookCategory, Is.Not.Null);
        //    Assert.That(addedBookCategory.CategoryId, Is.EqualTo(model.CategoryId));
        //}

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
        public async Task GetBookDetailsForUserAsync_ReturnsBookDetailsViewModel()
        {
            string bookId = FirstBook!.Id.ToString();
            var expectedEditions = new List<EditionsForBookDetailsViewModel> {
                /* Populate with expected editions */ 
            };
            var expectedCategoryName = FirstCategory!.Name;
            var expectedBookRating = 4.5m;
            var expectedPeopleReading = 1;

            editionServiceMock.Setup(m => m.GetEditionsForBookDetailsViewModelAsync(bookId)).ReturnsAsync(expectedEditions);
            categoryServiceMock.Setup(m => m.GetCategoryNameByBookIdAsync(bookId)).ReturnsAsync(expectedCategoryName);
            ratingServiceMock.Setup(m => m.GetAverageRatingForBookAsync(bookId)).ReturnsAsync(expectedBookRating);
            lendedBooksServiceMock.Setup(m => m.GetCountOfPeopleReadingTheBookAsync(bookId)).ReturnsAsync(expectedPeopleReading);

            // Act
            var result = await bookService.GetBookDetailsForUserAsync(bookId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookId, result.Id);
            // Add more assertions based on expected values and properties of BookDetailsViewModel
            Assert.AreEqual(expectedEditions, result.Editions);
            Assert.AreEqual(expectedCategoryName, result.CategoryName);
            Assert.AreEqual(expectedBookRating, result.Rating);
            Assert.AreEqual(expectedPeopleReading, result.PeopleReading);
        }
    }
}
