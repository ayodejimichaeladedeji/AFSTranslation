namespace AFSTranslator.Models
{
    public class RegistrationRequestViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 7)]
        public string Password { get; set; } = string.Empty;
    }
}