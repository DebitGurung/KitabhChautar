using System.ComponentModel.DataAnnotations.Schema;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public Genre Genre { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime PublishedDate { get; set; } = DateTime.UtcNow;
    public int Pages { get; set; }
    public int StockCount { get; set; }
    public string Synopsis { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;

    // ---------- Foreign Keys & Navigation Properties ----------
    public int? AdminId { get; set; }    // Foreign Key (Admin)
    [ForeignKey("AdminId")]
    public Admin? Admin { get; set; }    // Navigation Property (Admin)
    public int? MemberId { get; set; }   // Foreign Key (Member)
    [ForeignKey("MemberId")]
    public Member? Member { get; set; }  // Navigation Property (Member)
    public int? StaffId { get; set; }    // Foreign Key (Staff)
    [ForeignKey("StaffId")]
    public Staff? Staff { get; set; }    // Navigation Property (Staff)
}

