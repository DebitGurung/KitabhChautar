<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KitabhChauta.Model
{
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Genre_id { get; set; }

        [Required(ErrorMessage = "Author name is required")]
        [StringLength(100, ErrorMessage = "Author name cannot be longer than 100 characters")]
        public string Genre_Name { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}

=======
﻿//using kitabhChautari.Models;

//namespace KitabhChautari.Models
//{
//    public class Genre
//    {
//        public int GenreId { get; set; }
//        public string GenreName { get; set; }
//        public ICollection<Book> Books { get; set; } // Navigation property
//    }
//}
>>>>>>> f5451a52d1c4c87b33f69c61b45926a525e29c94
