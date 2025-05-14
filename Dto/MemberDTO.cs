<<<<<<< HEAD
﻿
using System.ComponentModel.DataAnnotations;
=======
﻿using System.ComponentModel.DataAnnotations;
using KitabhChautari.Enums;
>>>>>>> f5451a52d1c4c87b33f69c61b45926a525e29c94

namespace KitabhChautari.Dto
{
    public class MemberDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

<<<<<<< HEAD
    /// <summary>
    /// The first name of the member.
    /// </summary>
    [Required]
    [MaxLength(100)]
    [MinLength(2)]
    public required string FirstName { get; set; }

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
=======
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Phone]
        public string ContactNo { get; set; } = string.Empty;
>>>>>>> f5451a52d1c4c87b33f69c61b45926a525e29c94

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

<<<<<<< HEAD

=======
        public MembershipStatus MembershipStatus { get; set; } = MembershipStatus.Active;
>>>>>>> f5451a52d1c4c87b33f69c61b45926a525e29c94

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