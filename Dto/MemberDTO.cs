using KitabhChautari.Enums;
using System.ComponentModel.DataAnnotations;


public class MemberDto
{
    public int MemberId { get; set; } // Include for PUT requests (ID in URL + body)

    /// <summary>
    /// The first name of the member.
    /// </summary>
    [Required]
    [MaxLength(100)]
    [MinLength(2)]
    public string FirstName { get; set; }

    /// <summary>
    /// The last name of the member.
    /// </summary>
    [Required]
    [MaxLength(100)]
    [MinLength(2)]
    public string LastName { get; set; }

    /// <summary>
    /// The email address of the member.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// The date of birth of the member.
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// The membership status of the member.
    /// </summary>
    public MembershipStatus MembershipStatus { get; set; } = MembershipStatus.Active;

}
