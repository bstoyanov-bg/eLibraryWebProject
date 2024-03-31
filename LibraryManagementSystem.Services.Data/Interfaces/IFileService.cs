using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(string id, IFormFile file, string type);

        Task<string> UploadImageFileAsync(string id, IFormFile file, string type);

        Task<FileStreamResult> DownloadFileAsync(string id, string entityType);

        Task<string> GetFilePathAsync(string id, string entityType);

        Task<string> GetFileContentAsync(string filePath);
    }
}
