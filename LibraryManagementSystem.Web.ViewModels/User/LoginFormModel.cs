using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Web.ViewModels.User
{
    public class LoginFormModel
    {
        //[Required]
        //[EmailAddress]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; } = null!;

        // User is going to login with username!!!
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
