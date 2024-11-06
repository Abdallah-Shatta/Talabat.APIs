using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"(?=^.{6,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$",
                            ErrorMessage = "Password must have at least one uppercase, one lowercase, one digit, one non alphanumeric and 6 up to 10 characters")]
        public string Password { get; set; }

        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
