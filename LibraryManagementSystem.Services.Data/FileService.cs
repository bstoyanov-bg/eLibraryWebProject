using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

public class FileService : IFileService
{
    private readonly ELibraryDbContext dbContext;
    private readonly IWebHostEnvironment hostEnvironment;

    public FileService(ELibraryDbContext dbContext, IWebHostEnvironment hostEnvironment)
    {
        this.dbContext = dbContext;
        this.hostEnvironment = hostEnvironment;
    }

    public async Task<string> UploadFileAsync(string id, IFormFile file, string entityType)
    {
        Type type = entityType == nameof(Book) ? typeof(Book) : typeof(Edition);

        object? entity = await this.dbContext.FindAsync(type, Guid.Parse(id));

        // Relative path within the wwwroot directory
        string relativeFolderPath = Path.Combine("BookFiles", type.Name + "s");

        // Full physical path
        string uploadsFolder = Path.Combine(this.hostEnvironment.WebRootPath, relativeFolderPath);

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
        string filePath = Path.Combine(relativeFolderPath, uniqueFileName); // Relative file path

        // Save the file to the wwwroot folder
        string fullFilePath = Path.Combine(uploadsFolder, uniqueFileName);
        using (var stream = new FileStream(fullFilePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Update entity with the relative file path
        PropertyInfo? property = type.GetProperty("FilePath");
        if (property != null)
        {
            property.SetValue(entity, filePath);
            await this.dbContext.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("FilePath property not found!");
        }

        return filePath;
    }

    public async Task<string> UploadImageFileAsync(string id, IFormFile file, string entityType)
    {
        Type type = entityType == nameof(Book) ? typeof(Book) : typeof(Author);

        object? entity = await this.dbContext.FindAsync(type, Guid.Parse(id));

        // Relative path within the wwwroot directory
        string relativeFolderPath = Path.Combine("img", type.Name + "Covers");

        // Full physical path
        string uploadsFolder = Path.Combine(this.hostEnvironment.WebRootPath, relativeFolderPath);

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        string filePath = Path.Combine(relativeFolderPath, uniqueFileName); // Relative file path

        // Save the file to the wwwroot folder
        string fullFilePath = Path.Combine(uploadsFolder, uniqueFileName);
        using (var stream = new FileStream(fullFilePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Update entity with the file path
        PropertyInfo? property = type.GetProperty("ImageFilePath");
        if (property != null)
        {
            property.SetValue(entity, filePath);
            await this.dbContext.SaveChangesAsync();
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

        object? entity = await this.dbContext.FindAsync(type, Guid.Parse(id));

        PropertyInfo? property = type.GetProperty("FilePath");
        if (property == null)
        {
            throw new InvalidOperationException("FilePath property not found!");
        }

        string? relativeFilePath = (string?)property.GetValue(entity); // Relative file path

        string fullFilePath = Path.Combine(this.hostEnvironment.WebRootPath, relativeFilePath!); // Full file path

        if (string.IsNullOrEmpty(fullFilePath) || !System.IO.File.Exists(fullFilePath))
        {
            throw new FileNotFoundException("File not found!");
        }

        // Retrieve the file content
        FileStream fileStream = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read);

        // Return the file as a stream with content type set to text/plain for .txt files
        return new FileStreamResult(fileStream, "text/plain")
        {
            FileDownloadName = Path.GetFileName(fullFilePath)
        };
    }

    public async Task<string> GetFilePathAsync(string id, string entityType)
    {
        Type type = entityType == nameof(Book) ? typeof(Book) : typeof(Edition);

        object? entity = await this.dbContext.FindAsync(type, Guid.Parse(id));

        PropertyInfo? property = type.GetProperty("FilePath");
        if (property == null)
        {
            throw new InvalidOperationException("FilePath property not found!");
        }

        string? relativeFilePath = (string?)property.GetValue(entity); // Relative file path

        string fullFilePath = Path.Combine(this.hostEnvironment.WebRootPath, relativeFilePath!); // Full file path

        if (string.IsNullOrEmpty(fullFilePath) || !System.IO.File.Exists(fullFilePath))
        {
            throw new FileNotFoundException("File not found!");
        }

        return fullFilePath;
    }

    public async Task<string> GetFileContentAsync(string filePath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string fileContent = await reader.ReadToEndAsync();
                return fileContent;
            }
        }
        catch
        {
            throw;
        }
    }
}
