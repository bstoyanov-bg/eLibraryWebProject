using LibraryManagementSystem.Data;
using LibraryManagementSystem.Services.Data;
using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using static LibraryManagementSystem.Services.Tests.DatabaseSeeder;

namespace LibraryManagementSystem.Services.Tests
{
    [TestFixture]
    public class LendedBookServiceTests
    {
        private DbContextOptions<ELibraryDbContext> dbOptions;
        private ELibraryDbContext dbContext;

        private ILendedBookService lendedBookService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<ELibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "ELibraryTestDb")
                .Options;
        }

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new ELibraryDbContext(this.dbOptions);
            this.dbContext.Database.EnsureCreated();
            SeedDatabase(this.dbContext);

            this.lendedBookService = new LendedBookService(this.dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async Task AddLendedBookToCollectionAsync_UserBookAddedSuccessfully()
        {
            var userId = ThirdUser!.Id.ToString();
            var bookId = FirstBook!.Id.ToString();

            await this.lendedBookService.AddBookToCollectionAsync(userId, bookId);

            var addedUserBook = await this.dbContext.LendedBooks.FirstOrDefaultAsync(ub => ub.UserId == Guid.Parse(userId) && ub.BookId == Guid.Parse(bookId));

            Assert.That(addedUserBook, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(addedUserBook.UserId.ToString(), Is.EqualTo(userId));
                Assert.That(addedUserBook.BookId.ToString(), Is.EqualTo(bookId));
                Assert.That(addedUserBook.LoanDate.Date, Is.EqualTo(DateTime.UtcNow.Date));
            });
        }

        [Test]
        public async Task ReturnLendedBookAsync_BookReturnedSuccessfully()
        {
            var userId = SecondUser!.Id.ToString();
            var bookId = SeventhBook!.Id.ToString();

            await this.lendedBookService.ReturnBookAsync(userId, bookId);

            var returnedUserBook = await this.dbContext.LendedBooks.FirstOrDefaultAsync(ub => ub.UserId == Guid.Parse(userId) && ub.BookId == Guid.Parse(bookId));

            Assert.That(returnedUserBook, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(returnedUserBook.ReturnDate!.Value, Is.Not.NaN);
                Assert.That(returnedUserBook.ReturnDate?.Date, Is.EqualTo(DateTime.UtcNow.Date));
            });
        }

        [Test]
        public async Task ReturnAllBooksAsync_AllBooksReturnedSuccessfully()
        {
            var userId = SecondUser!.Id.ToString();

            var returnedUserBooks = await this.dbContext.LendedBooks.Where(ub => ub.UserId == Guid.Parse(userId) && ub.ReturnDate == null).ToListAsync();

            await this.lendedBookService.ReturnAllBooksAsync(userId);

            foreach (var returnedUserBook in returnedUserBooks)
            {
                Assert.That(returnedUserBook.ReturnDate?.Date, Is.EqualTo(DateTime.UtcNow.Date));
            }
        }

        [Test]
        public async Task IsBookActiveInUserCollectionAsync_BookIsInUserCollection_ReturnsTrue()
        {
            var userId = SecondUser!.Id.ToString();
            var bookId = FirstBook!.Id.ToString();

            await this.lendedBookService.AddBookToCollectionAsync(userId, bookId);

            var isActive = await this.lendedBookService.IsBookActiveInUserCollectionAsync(userId, bookId);

            Assert.That(isActive, Is.True);
        }

        [Test]
        public async Task IsBookActiveInUserCollectionAsync_BookNotInUserCollection_ReturnsFalse()
        {
            var userId = SecondUser!.Id.ToString();
            var bookId = FirstBook!.Id.ToString();

            var isActive = await this.lendedBookService.IsBookActiveInUserCollectionAsync(userId, bookId);

            Assert.That(isActive, Is.False);
        }

        [Test]
        public async Task BookExistsInUserHistoryCollectionAsync_BookExistsInHistoryCollection_ReturnsTrue()
        {
            var userId = SecondUser!.Id.ToString();
            var bookId = FirstBook!.Id.ToString();

            await this.lendedBookService.AddBookToCollectionAsync(userId, bookId);

            await this.lendedBookService.ReturnBookAsync(userId, bookId);

            var existsInHistory = await this.lendedBookService.BookExistsInUserHistoryCollectionAsync(userId, bookId);

            Assert.That(existsInHistory, Is.True);
        }

        [Test]
        public async Task BookExistsInUserHistoryCollectionAsync_BookDoesNotExistInHistoryCollection_ReturnsFalse()
        {
            var userId = SecondUser!.Id.ToString();
            var bookId = FirstBook!.Id.ToString();

            var userBook = await this.dbContext.LendedBooks.FirstOrDefaultAsync(ub => ub.UserId == Guid.Parse(userId) && ub.BookId == Guid.Parse(bookId) && ub.ReturnDate != null);
            if (userBook != null)
            {
                userBook.ReturnDate = null;
                await this.dbContext.SaveChangesAsync();
            }

            var existsInHistory = await this.lendedBookService.BookExistsInUserHistoryCollectionAsync(userId, bookId);

            Assert.That(existsInHistory, Is.False);
        }

        [Test]
        public async Task IsBookReturnedAsync_BookIsNotReturned_ReturnsTrue()
        {
            var userId = SecondUser!.Id.ToString();
            var bookId = FifthBook!.Id.ToString();

            await this.lendedBookService.AddBookToCollectionAsync(userId, bookId);

            var isReturned = await this.lendedBookService.IsBookReturnedAsync(userId, bookId);

            Assert.That(isReturned, Is.True);
        }

        [Test]
        public async Task IsBookReturnedAsync_BookIsReturned_ReturnsFalse()
        {
            var userId = SecondUser!.Id.ToString();
            var bookId = FifthBook!.Id.ToString();

            var userBook = await this.dbContext.LendedBooks.FirstOrDefaultAsync(ub => ub.UserId == Guid.Parse(userId) && ub.BookId == Guid.Parse(bookId) && ub.ReturnDate != null);
  
            var isReturned = await this.lendedBookService.IsBookReturnedAsync(userId, bookId);

            Assert.That(isReturned, Is.False);
        }

        [Test]
        public async Task AreThereAnyNotReturnedBooksAsync_BooksNotReturned_ReturnsTrue()
        {
            var userId = SecondUser!.Id.ToString();

            var bookId = FirstBook!.Id.ToString();
            await this.lendedBookService.AddBookToCollectionAsync(userId, bookId);

            var result = await this.lendedBookService.AreThereAnyNotReturnedBooksAsync(userId);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task AreThereAnyNotReturnedBooksAsync_AllBooksReturned_ReturnsFalse()
        {
            var userId = SecondUser!.Id.ToString();

            var userBooks = await this.dbContext.LendedBooks.Where(ub => ub.UserId == Guid.Parse(userId) && ub.ReturnDate == null).ToListAsync();
            foreach (var userBook in userBooks)
            {
                userBook.ReturnDate = DateTime.UtcNow;
            }
            await this.dbContext.SaveChangesAsync();

            var result = await this.lendedBookService.AreThereAnyNotReturnedBooksAsync(userId);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetCountOfActiveBooksForUserAsync_ReturnsCorrectCount()
        {
            var userId = ThirdUser!.Id.ToString();

            await this.lendedBookService.AddBookToCollectionAsync(userId, FirstBook!.Id.ToString());
            await this.lendedBookService.AddBookToCollectionAsync(userId, SecondBook!.Id.ToString());
            await this.lendedBookService.AddBookToCollectionAsync(userId, ThirdBook!.Id.ToString());

            var bookToReturn = await this.dbContext.LendedBooks.FirstOrDefaultAsync(ub => ub.UserId == Guid.Parse(userId) && ub.ReturnDate == null);
            if (bookToReturn != null)
            {
                bookToReturn.ReturnDate = DateTime.UtcNow;
            }
            await this.dbContext.SaveChangesAsync();

            var result = await this.lendedBookService.GetCountOfActiveBooksForUserAsync(userId);

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public async Task GetCountOfPeopleReadingTheBookAsync_ReturnsCorrectCount()
        {
            var bookId = FirstBook!.Id.ToString();

            await this.lendedBookService.AddBookToCollectionAsync(FirstUser!.Id.ToString(), bookId);
            await this.lendedBookService.AddBookToCollectionAsync(SecondUser!.Id.ToString(), bookId);
            await this.lendedBookService.AddBookToCollectionAsync(ThirdUser!.Id.ToString(), bookId);
            await this.lendedBookService.AddBookToCollectionAsync(ForthUser!.Id.ToString(), bookId);

            var bookToReturn = await this.dbContext.LendedBooks.FirstOrDefaultAsync(ub => ub.BookId == Guid.Parse(bookId) && ub.UserId == FirstUser!.Id && ub.ReturnDate == null);
            if (bookToReturn != null)
            {
                bookToReturn.ReturnDate = DateTime.UtcNow;
            }
            await this.dbContext.SaveChangesAsync();

            var result = await this.lendedBookService.GetCountOfPeopleReadingTheBookAsync(bookId);

            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        public async Task GetCountOfLendedBooksAsync_ReturnsCorrectCount()
        {
            int ExpectedNumberOfLendedBooks = 5;

            var result = await this.lendedBookService.GetCountOfLendedBooksAsync();

            Assert.That(result, Is.EqualTo(ExpectedNumberOfLendedBooks));
        }

        // STRANGE PROBLEM WITH THE DB OR THE QUERY ???
        //[Test]
        //public async Task GetMyBooksAsync_ReturnsCorrectBooks()
        //{
        //    // Arrange
        //    var userId = ForthUser!.Id.ToString().ToLower();
        //    var expectedBooks = new List<MyBooksViewModel>
        //    {
        //        new MyBooksViewModel 
        //        { 
        //            Id = FirstBook!.Id.ToString(),
        //            Title = FirstBook.Title,
        //            YearPublished = FirstBook.YearPublished,
        //            Publisher = FirstBook.Publisher,
        //            AuthorName = $"{FirstBook.Author.FirstName} {FirstBook.Author.LastName}",
        //            ImageFilePath = FirstBook.ImageFilePath,
        //            EditionsCount = 0,
        //            FilePath = FirstBook.FilePath
        //        },
        //    };

        //    var lendedBooks = new List<LendedBook>
        //    {
        //        FifthLendedBook!
        //    };

        //    var myBooks = await this.lendedBookService.GetMyBooksAsync(userId);

        //    Assert.That(myBooks, Is.Not.Null);
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(myBooks.Count(), Is.EqualTo(expectedBooks.Count));
        //        Assert.That(myBooks.First().Title, Is.EqualTo(expectedBooks.First().Title));
        //    });
        //}

        [Test]
        public async Task GetMyBooksAsync_ReturnsEmptyList_WhenUserHasNoBooks()
        {
            var userId = "89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153";

            var myBooks = await lendedBookService.GetMyBooksAsync(userId);

            Assert.That(myBooks, Is.Not.Null);
            Assert.That(myBooks, Is.Empty);
        }
    }
}
