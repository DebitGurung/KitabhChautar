using System.ComponentModel.DataAnnotations;
using KitabhChauta.Enum;

namespace KitabhChauta.Dto
{
    public class BookDTO
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN is required")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative")]
        public decimal Price { get; set; }

        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

        [Range(0, int.MaxValue, ErrorMessage = "Pages must be non-negative")]
        public int Pages { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock count must be non-negative")]
        public int StockCount { get; set; }

        public string Synopsis { get; set; } = string.Empty;

        public string CoverImageUrl { get; set; } = string.Empty;

        public bool IsOnSale { get; set; } = false;

        [Range(0, 1, ErrorMessage = "Discount must be between 0 (0%) and 1 (100%)")]
        public decimal? DiscountPercentage { get; set; }

        public Category? Category { get; set; }

        [Required(ErrorMessage = "Author ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Author ID must be a positive integer")]
        public int Author_id { get; set; }

        [Required(ErrorMessage = "Genre ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Genre ID must be a positive integer")]
        public int Genre_id { get; set; }

        [Required(ErrorMessage = "Publisher ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Publisher ID must be a positive integer")]
        public int Publisher_id { get; set; }
    }
}