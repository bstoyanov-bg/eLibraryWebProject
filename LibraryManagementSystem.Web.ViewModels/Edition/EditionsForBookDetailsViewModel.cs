using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Web.ViewModels.Edition
{
    public class EditionsForBookDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public string Version { get; set; } = null!;

        public string Publisher { get; set; } = null!;

        public DateOnly EditionYear { get; set; }

        public string? FilePath { get; set; }
    }
}
