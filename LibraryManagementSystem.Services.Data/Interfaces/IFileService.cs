using Microsoft.AspNetCore.Http;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFile(string id, IFormFile file, string type);
    }
}
