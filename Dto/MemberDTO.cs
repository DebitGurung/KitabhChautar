
﻿
using System.ComponentModel.DataAnnotations;




namespace KitabhChautari.Dto
{
    public class MemberDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;


   
    /// <summary>
    /// The last name of the member.
    /// </summary>
    [Required]
    [MaxLength(100)]
    [MinLength(2)]
    public required string LastName { get; set; }

    /// <summary>
    /// The email address of the member.
    /// </summary>
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

       

        [Required]
        [MaxLength(20)]
        [Phone]
        public string ContactNo { get; set; } = string.Empty;


        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }


        [Required]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{}|;:,.<>?]).{12,}",
            ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}