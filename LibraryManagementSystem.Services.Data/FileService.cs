using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class FileService : IFileService
{
    private readonly ELibraryDbContext dbContext;

    public FileService(ELibraryDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<string> UploadFileAsync(string id, IFormFile file, string entityType)
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

    public async Task<FileStreamResult> DownloadFileAsync(string id, string entityType)
    {
        Type type = entityType == nameof(Book) ? typeof(Book) : typeof(Edition);

        var entity = await this.dbContext.FindAsync(type, Guid.Parse(id));

        var property = type.GetProperty("FilePath");
        if (property == null)
        {
            throw new InvalidOperationException("FilePath property not found!");
        }

        var filePath = (string?)property.GetValue(entity); // Note the use of string? for filePath

        if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found!");
        }

        // Retrieve the file content
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        // Return the file as a stream with content type set to text/plain for .txt files
        return new FileStreamResult(fileStream, "text/plain")
        {
            FileDownloadName = Path.GetFileName(filePath)
        };
    }

    public async Task<string> GetFilePathAsync(string id, string entityType)
    {
        Type type = entityType == nameof(Book) ? typeof(Book) : typeof(Edition);

        var entity = await this.dbContext.FindAsync(type, Guid.Parse(id));

        var property = type.GetProperty("FilePath");
        if (property == null)
        {
            throw new InvalidOperationException("FilePath property not found!");
        }

        var filePath = (string?)property.GetValue(entity);

        if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found!");
        }

        return filePath;
    }

    public async Task<string> GetFileContentAsync(string filePath)
    {
        try
        {
            using (var reader = new StreamReader(filePath))
            {
                string fileContent = await reader.ReadToEndAsync();
                return fileContent;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
            throw;
        }
    }
}
