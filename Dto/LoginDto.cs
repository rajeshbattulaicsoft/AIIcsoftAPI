using System.ComponentModel.DataAnnotations;

namespace AIIcsoftAPI.Dto
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
