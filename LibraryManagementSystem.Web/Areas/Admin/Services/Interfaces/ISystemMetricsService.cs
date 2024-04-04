using LibraryManagementSystem.Web.Areas.Admin.ViewModels.SystemMetrtics;

namespace LibraryManagementSystem.Web.Areas.Admin.Services.Interfaces
{
    public interface ISystemMetricsService
    {
        Task<SystemMetricsViewModel> GetSystemMetricsAsync();
    }
}
