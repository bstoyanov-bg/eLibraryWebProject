using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment environment;
    private readonly ELibraryDbContext dbContext;

    public FileService(IWebHostEnvironment environment, ELibraryDbContext dbContext)
    {
        this.environment = environment;
        this.dbContext = dbContext;
    }

    public async Task<string> UploadFile(string id, IFormFile file, string entityType)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("No file uploaded.");
        }

        if (!file.FileName.EndsWith(".txt"))
        {
            throw new ArgumentException("Only .txt files are allowed.");
        }

        //Check Assemblies

        //Type? textType;
        //string typeName = $"LibraryManagementSystem.Data.Models.{entityType}";
        //var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        //foreach (var assembly in assemblies)
        //{
        //    textType = assembly.GetType(typeName);
        //    if (textType != null)
        //        break;
        //}

        Type? type;

        if (entityType == "Book")
        {
            type = typeof(Book);
        }
        else 
        {
            type = typeof(Edition);
        }

        // Cannot get the type this way ???? :(
        //var typeStr = $"LibraryManagementSystem.Data.Models.{entityType}, LibraryManagementSystem";
        //Type? type = Type.GetType(typeStr);

        if (type == null)
        {
            throw new ArgumentException("Invalid entity type.");
        }

        var entity = await dbContext.FindAsync(type, Guid.Parse(id));
        if (entity == null)
        {
            throw new InvalidOperationException($"{type.Name} not found.");
        }

        var uploadsFolder = Path.Combine(environment.WebRootPath, $"BookFiles\\{type.Name}s");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var filePath = Path.Combine(uploadsFolder, file.FileName);
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
            throw new InvalidOperationException("FilePath property not found.");
        }

        return filePath;
    }
}
