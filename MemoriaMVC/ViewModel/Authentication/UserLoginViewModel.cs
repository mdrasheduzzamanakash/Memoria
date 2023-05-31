using System.ComponentModel.DataAnnotations;

namespace MemoriaMVC.ViewModel.Authentication
{
    public class UserLoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
