namespace MemoriaMVC.ViewModel.HomePageViewModel
{
    public class HomeIndexViewModel
    {
        public string Id { get; set; }
        public int Status { get; set; } = 1;
        public DateTime AddedDateAndTime { get; set; }
        public DateTime UpdatedDateAndTime { get; set; }
        public string? UpdatedBy { get; set; }
        public string? AddedBy { get; set; }
        public string? FileFormat { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Image { get; set; }
    }
}
