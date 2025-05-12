using System.ComponentModel.DataAnnotations;
using KitabhChautari;

namespace KitabhChautari.Dtos
{
    public class BookDto
    {
        public int BookId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        public Genre Genre { get; set; }

        [Required]
        [MaxLength(13)]
        public string ISBN { get; set; } = string.Empty;

        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

        [Range(1, 10000)]
        public int Pages { get; set; }

        [Range(0, int.MaxValue)]
        public int StockCount { get; set; }

        [MaxLength(1000)]
        public string Synopsis { get; set; } = string.Empty;

        [MaxLength(500)]
        public string CoverImageUrl { get; set; } = string.Empty;

        public int? AdminId { get; set; }
        public int? MemberId { get; set; }
        public int? StaffId { get; set; }
    }
}