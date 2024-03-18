using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.AspNetCore.Http;

public class FileService : IFileService
{
    //private readonly IWebHostEnvironment environment;
    private readonly ELibraryDbContext dbContext;

    public FileService(ELibraryDbContext dbContext /*IWebHostEnvironment environment*/)
    {
        //this.environment = environment;
        this.dbContext = dbContext;
    }

    public async Task<string> UploadFile(string id, IFormFile file, string entityType)
    {
        Type type = entityType == nameof(Book) ? typeof(Book) : typeof(Edition);

        var entity = await dbContext.FindAsync(type, Guid.Parse(id));

        var uploadsFolder = $"BookFiles/{type.Name}s/";
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var filePath = $"{uploadsFolder}{file.FileName}";
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Update entity with the file path
        var property = type.GetProperty("FilePath");
        if (property != null)
        {
            property.SetValue(entity, filePath);
            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("FilePath property not found!");
        }

        return filePath;
    }
}
