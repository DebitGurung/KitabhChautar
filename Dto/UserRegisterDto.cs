using System.ComponentModel.DataAnnotations;

namespace kitabhChautari.Dto
{
    public class UserRegisterDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Username { get; set; } = null!;

        [Required]
        [Phone]
        public string ContactNo { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = null!;
    }
}