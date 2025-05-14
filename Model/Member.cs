<<<<<<< HEAD
﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
=======
﻿using KitabhChautari.Enums;
>>>>>>> f5451a52d1c4c87b33f69c61b45926a525e29c94

namespace kitabhChautari.Models
{
<<<<<<< HEAD
    public int MemberId { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;



  
 
}

=======
    public class Member
    {
        public int MemberId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; } // Made nullable
        public DateTime? DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public MembershipStatus MembershipStatus { get; set; }
        public string ContactNo { get; set; }
        public bool IsStaff { get; set; }
    }
}
>>>>>>> f5451a52d1c4c87b33f69c61b45926a525e29c94
