using System.ComponentModel.DataAnnotations;

namespace MemoriaMVC.ViewModel.Authentication
{
    public class UserRegistrationViewModel
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
