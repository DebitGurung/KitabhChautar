using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using kitabhChauta.Models;

namespace KitabhChauta.Model
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public Member Member { get; set; } = null!;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalPrice { get; set; }

        public bool IsCompleted { get; set; } = false;

        public bool IsCanceled { get; set; } = false; // New field

        public DateTime? CancellationDate { get; set; } // New field

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}