using System.ComponentModel.DataAnnotations;

namespace KitabhChauta.Dto
{
    public class AuthorDTO
    {
        public int Author_id { get; set; }

        [Required(ErrorMessage = "Author name is required")]
        [StringLength(100, ErrorMessage = "Author name cannot be longer than 100 characters")]
        public string Author_Name { get; set; } = string.Empty;
    }
}