using System.ComponentModel.DataAnnotations;


public class UserDto
{
    public int UserId { get; set; } // Include for PUT requests (ID in URL + body)

    [Required]
    [MaxLength(100)] // Match model's [MaxLength(100)] for consistency
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
