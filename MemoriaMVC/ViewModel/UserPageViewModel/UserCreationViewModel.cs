using Microsoft.Build.Framework;

namespace MemoriaMVC.ViewModel.UserPageViewModel
{
    public class UserCreationViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
