using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Author;
using LibraryManagementSystem.Web.ViewModels.Author.Enums;
using LibraryManagementSystem.Web.ViewModels.Book;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Globalization;
using static LibraryManagementSystem.Services.Tests.DatabaseSeeder;

namespace LibraryManagementSystem.Services.Tests
{
    [TestFixture]
    public class AuthorServiceTests
    {
        private DbContextOptions<ELibraryDbContext> dbOptions;
        private ELibraryDbContext dbContext;
        private Mock<IBookService> bookServiceMock;
        private IAuthorService authorService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<ELibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "ELibraryTestDb")
                .Options;

            this.bookServiceMock = new Mock<IBookService>();
        }

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new ELibraryDbContext(this.dbOptions);
            this.dbContext.Database.EnsureCreated();
            SeedDatabase(this.dbContext);

            this.authorService = new AuthorService(this.dbContext, new Lazy<IBookService>(() => this.bookServiceMock.Object));
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
            this.bookServiceMock.Reset();
        }

        [Test]
        public async Task DeleteAuthorAsync_ValidAuthorId_SetsIsDeletedToTrue()
        {
            string authorId = SecondAuthor!.Id.ToString();

            await this.authorService.DeleteAuthorAsync(authorId);

            var deletedAuthor = await this.dbContext.Authors.FindAsync(Guid.Parse(authorId));

            Assert.That(deletedAuthor!.IsDeleted, Is.True);
        }

        [Test]
        public async Task EditAuthorAsync_ValidAuthorId_UpdatesAuthorFields()
        {
            string authorId = FirstAuthor!.Id.ToString();
            var model = new AuthorFormModel
            {
                FirstName = "Stephen W.",
                LastName = "Hawking",
                Biography = "Updated biography...",
                BirthDate = new DateOnly(1942, 1, 8),
                DeathDate = new DateOnly(2018, 3, 14),
                Nationality = "British",
            };

            var editedAuthor = await this.authorService.EditAuthorAsync(authorId, model);

            Assert.That(editedAuthor, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(editedAuthor.FirstName, Is.EqualTo(model.FirstName));
                Assert.That(editedAuthor.LastName, Is.EqualTo(model.LastName));
                Assert.That(editedAuthor.Biography, Is.EqualTo(model.Biography));
                Assert.That(editedAuthor.BirthDate, Is.EqualTo(model.BirthDate));
                Assert.That(editedAuthor.DeathDate, Is.EqualTo(model.DeathDate));
                Assert.That(editedAuthor.Nationality, Is.EqualTo(model.Nationality));
            });
        }

        [Test]
        public async Task AddAuthorAsync_WithValidModel_AddsAuthorToDatabase()
        {
            var authorFormModel = new AuthorFormModel
            {
                FirstName = "Test",
                LastName = "Testov",
                Biography = "Biography of Test Testov",
                BirthDate = new DateOnly(1980, 1, 1),
                DeathDate = new DateOnly(2050, 12, 31),
                Nationality = "Swedish",
                ImageFilePath = "path/to/image.jpg"
            };

            var addedAuthor = await this.authorService.AddAuthorAsync(authorFormModel);

            Assert.That(addedAuthor, Is.Not.Null);
            Assert.That(addedAuthor.FirstName, Is.EqualTo(authorFormModel.FirstName));
            Assert.That(addedAuthor.LastName, Is.EqualTo(authorFormModel.LastName));
            Assert.That(addedAuthor.Biography, Is.EqualTo(authorFormModel.Biography));
            Assert.That(addedAuthor.BirthDate, Is.EqualTo(authorFormModel.BirthDate));
            Assert.That(addedAuthor.DeathDate, Is.EqualTo(authorFormModel.DeathDate));
            Assert.That(addedAuthor.Nationality, Is.EqualTo(authorFormModel.Nationality));
            Assert.That(addedAuthor.ImageFilePath, Is.EqualTo(authorFormModel.ImageFilePath));

            // Ensure author is added to the database
            var retrievedAuthor = await this.dbContext.Authors.FindAsync(addedAuthor.Id);
            Assert.That(retrievedAuthor, Is.Not.Null);
            Assert.That(retrievedAuthor.Id, Is.EqualTo(addedAuthor.Id));
        }

        [Test]
        public async Task GetAuthorByIdAsync_ExistingAuthorId_ReturnsAuthor()
        {
            string existingAuthorId = ThirdAuthor!.Id.ToString();

            var author = await this.authorService.GetAuthorByIdAsync(existingAuthorId);

            Assert.That(author, Is.Not.Null);
            Assert.That(author.FirstName, Is.EqualTo("Franklin"));
            Assert.That(author.LastName, Is.EqualTo("Herbert"));
            Assert.That(author.Biography, Is.EqualTo("Franklin Patrick Herbert Jr. was an American science fiction author best known for the 1965 novel Dune and its five sequels. Though he became famous for his novels, he also wrote short stories and worked as a newspaper journalist, photographer, book reviewer, ecological consultant, and lecturer.\r\n\r\nThe Dune saga, set in the distant future, and taking place over millennia, explores complex themes, such as the long-term survival of the human species, human evolution, planetary science and ecology, and the intersection of religion, politics, economics and power in a future where humanity has long since developed interstellar travel and settled many thousands of worlds. Dune is the best-selling science fiction novel of all time, and the entire series is considered to be among the classics of the genre."));
            Assert.That(author.BirthDate, Is.EqualTo(DateOnly.ParseExact("08.10.1920", "dd.MM.yyyy", CultureInfo.InvariantCulture)));
            Assert.That(author.DeathDate, Is.EqualTo(DateOnly.ParseExact("11.02.1986", "dd.MM.yyyy", CultureInfo.InvariantCulture)));
            Assert.That(author.Nationality, Is.EqualTo("American"));
            Assert.That(author.ImageFilePath, Is.EqualTo("img/AuthorCovers/11da75c5-3f40-4256-9785-dd9101af213b_Frank-Herbert-Image.jpg"));
            Assert.That(author.IsDeleted, Is.EqualTo(false));
        }

        [Test]
        public async Task GetAuthorByIdAsync_NonExistingAuthorId_ReturnsNull()
        {
            var nonExistingAuthorId = "non-existing-id";

            var author = await this.authorService.GetAuthorByIdAsync(nonExistingAuthorId);

            Assert.That(author, Is.Null);
        }

        [Test]
        public async Task GetAuthorForEditByIdAsync_ValidAuthorId_ReturnsAuthorFormModel()
        {
            string validAuthorId = ThirdAuthor!.Id.ToString();

            var expectedAuthor = ThirdAuthor;

            var result = await this.authorService.GetAuthorForEditByIdAsync(validAuthorId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.FirstName, Is.EqualTo(expectedAuthor.FirstName));
            Assert.That(result.LastName, Is.EqualTo(expectedAuthor.LastName));
            Assert.That(result.Biography, Is.EqualTo(expectedAuthor.Biography));
            Assert.That(result.BirthDate, Is.EqualTo(expectedAuthor.BirthDate));
            Assert.That(result.DeathDate, Is.EqualTo(expectedAuthor.DeathDate));
            Assert.That(result.Nationality, Is.EqualTo(expectedAuthor.Nationality));
            Assert.That(result.ImageFilePath, Is.EqualTo(expectedAuthor.ImageFilePath));
        }

        [Test]
        public async Task GetAuthorForEditByIdAsync_InvalidAuthorId_ReturnsNull()
        {
            string invalidAuthorId = "invalid-author-id";

            var result = await this.authorService.GetAuthorForEditByIdAsync(invalidAuthorId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetAuthorDetailsForUserAsync_ValidAuthorId_ReturnsAuthorDetailsViewModel()
        {
            // Arrange
            string validAuthorId = ThirdAuthor!.Id.ToString();
            var expectedAuthor = ThirdAuthor;

            var expectedBooks = new List<BooksForAuthorDetailsViewModel>
            {
                new BooksForAuthorDetailsViewModel
                {
                    Id = FirstBook!.Id.ToString(),
                    Title = FirstBook!.Title,
                    ISBN = FirstBook!.ISBN,
                    YearPublished = FirstBook!.YearPublished,
                    Description = FirstBook!.Description,
                    Publisher = FirstBook!.Publisher,
                    ImageFilePath = FirstBook!.ImageFilePath
                },
                new BooksForAuthorDetailsViewModel
                {
                    Id = SecondBook!.Id.ToString(),
                    Title = SecondBook!.Title,
                    ISBN = SecondBook!.ISBN,
                    YearPublished = SecondBook!.YearPublished,
                    Description = SecondBook!.Description,
                    Publisher = SecondBook!.Publisher,
                    ImageFilePath = SecondBook!.ImageFilePath
                }
            };

            this.bookServiceMock.Setup(m => m.GetBooksForAuthorDetailsAsync(validAuthorId)).ReturnsAsync(expectedBooks);

            var result = await this.authorService.GetAuthorDetailsForUserAsync(validAuthorId);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.FirstName, Is.EqualTo(expectedAuthor.FirstName));
                Assert.That(result.LastName, Is.EqualTo(expectedAuthor.LastName));
                Assert.That(result.Biography, Is.EqualTo(expectedAuthor.Biography));
                Assert.That(result.BirthDate, Is.EqualTo(expectedAuthor.BirthDate));
                Assert.That(result.DeathDate, Is.EqualTo(expectedAuthor.DeathDate));
                Assert.That(result.Nationality, Is.EqualTo(expectedAuthor.Nationality));
                Assert.That(result.ImageFilePath, Is.EqualTo(expectedAuthor.ImageFilePath));
            });

            CollectionAssert.AreEqual(expectedBooks, result.Books);
        }

        [Test]
        public async Task GetAllAuthorsFilteredAndPagedAsync_ReturnsExpectedResult()
        {

            var queryModel = new AllAuthorsQueryModel()
            {
                SearchString = "French",
                AuthorSorting = AuthorSorting.Newest,
                CurrentPage = 1,
                AuthorsPerPage = 10,
            };

            var result = await this.authorService.GetAllAuthorsFilteredAndPagedAsync(queryModel);
            int actualCount = await this.dbContext.Authors.CountAsync(c => c.Nationality.Contains(queryModel.SearchString));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TotalAuthorsCount, Is.EqualTo(actualCount));
        }

        [Test]
        public async Task AuthorExistByNameAndNationalityAsync_AuthorExists_ReturnsTrue()
        {
            LibraryManagementSystem.Data.Models.Author? author = FirstAuthor;

            bool authorExists = await this.authorService
                .AuthorExistByNameAndNationalityAsync(author!.FirstName, author.LastName, author.Nationality);

            Assert.That(authorExists, Is.True);
        }

        [Test]
        public async Task AuthorExistByNameAndNationalityAsync_AuthorDoesNotExist_ReturnsFalse()
        {
            string firstName = "Gosho";
            string lastName = "Petrov";
            string nationality = "Serbian";

            bool authorExists = await this.authorService.AuthorExistByNameAndNationalityAsync(firstName, lastName, nationality);

            Assert.That(authorExists, Is.False);
        }

        [Test]
        public async Task AuthorExistByIdAsync_AuthorExists_ReturnsTrue()
        {
            Author? author = FirstAuthor;

            bool authorExists = await this.authorService.AuthorExistByIdAsync(author!.Id.ToString());

            Assert.That(authorExists, Is.True);
        }

        [Test]
        public async Task AuthorExistByIdAsync_AuthorDoesNotExist_ReturnsFalse()
        {
            var authorId = Guid.NewGuid().ToString();

            bool authorExists = await this.authorService.AuthorExistByIdAsync(authorId);

            Assert.That(authorExists, Is.False);
        }

        [Test]
        public async Task GetCountOfActiveAuthorsAsync_ReturnsCountOfActiveAuthors()
        {
            int expectedCount = 3;

            int actualCount = await this.authorService.GetCountOfActiveAuthorsAsync();

            Assert.That(actualCount, Is.EqualTo(expectedCount));
        }

        [Test]
        public async Task GetCountOfDeletedAuthorsAsync_ReturnsCountOfDeletedAuthors()
        {
            int expectedCount = 2;

            await this.authorService.DeleteAuthorAsync(FirstAuthor!.Id.ToString());
            await this.authorService.DeleteAuthorAsync(SecondAuthor!.Id.ToString());

            int actualCount = await this.authorService.GetCountOfDeletedAuthorsAsync();

            Assert.That(actualCount, Is.EqualTo(expectedCount));
        }

        [Test]
        public async Task GetAllAuthorsForListAsync_ReturnsAllActiveAuthors()
        {
            var expectedAuthors = new List<AuthorsSelectForBookFormModel>
            {
                new AuthorsSelectForBookFormModel { Id = FirstAuthor!.Id.ToString(), Name = FirstAuthor!.FirstName + " " + FirstAuthor!.LastName, Nationality = FirstAuthor!.Nationality },
                new AuthorsSelectForBookFormModel { Id = SecondAuthor!.Id.ToString(), Name = SecondAuthor!.FirstName + " " + SecondAuthor!.LastName, Nationality = SecondAuthor!.Nationality },
                new AuthorsSelectForBookFormModel { Id = ThirdAuthor!.Id.ToString(), Name = ThirdAuthor!.FirstName + " " + ThirdAuthor!.LastName, Nationality = ThirdAuthor!.Nationality },
            };

            var actualAuthors = await this.authorService.GetAllAuthorsForListAsync();

            Assert.That(expectedAuthors.Count(), Is.EqualTo(actualAuthors.Count()));
            CollectionAssert.AllItemsAreNotNull(actualAuthors);
            //CollectionAssert.AreEquivalent(expectedAuthors, actualAuthors.ToList());
        }

    }
}