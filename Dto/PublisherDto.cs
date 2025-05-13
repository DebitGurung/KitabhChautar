using System.ComponentModel.DataAnnotations;

namespace KitabhChauta.Dto
{
    public class PublisherDTO
    {
        public int Publisher_id { get; set; }

        [Required(ErrorMessage = "Publisher name is required")]
        [StringLength(100, ErrorMessage = "Publisher name cannot be longer than 100 characters")]
        public string Publisher_Name { get; set; } = string.Empty;
    }
}