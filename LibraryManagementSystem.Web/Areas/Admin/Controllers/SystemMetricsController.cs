using LibraryManagementSystem.Web.Areas.Admin.Services.Interfaces;
using LibraryManagementSystem.Web.Areas.Admin.ViewModels.SystemMetrtics;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Web.Areas.Admin.Controllers
{
    public class SystemMetricsController : BaseAdminController
    {
        private readonly ISystemMetricsService systemMetricsService;

        public SystemMetricsController(ISystemMetricsService systemMetricsService)
        {
            this.systemMetricsService = systemMetricsService;
        }

        [HttpGet]
        public async Task<IActionResult> Metrics()
        {
            SystemMetricsViewModel model = await this.systemMetricsService.GetSystemMetricsAsync();

            return this.View(model);
        }
    }
}
