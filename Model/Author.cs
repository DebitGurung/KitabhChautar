<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitabhChauta.Model
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Author_id { get; set; }

        [Required(ErrorMessage = "Author name is required")]
        [StringLength(100, ErrorMessage = "Author name cannot be longer than 100 characters")]
        public string Author_Name { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
=======
﻿//using kitabhChautari.Models;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace KitabhChauta.Model
//{
//    public class Author
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Author_id { get; set; }

//        [Required(ErrorMessage = "Author name is required")]
//        [StringLength(100, ErrorMessage = "Author name cannot be longer than 100 characters")]
//        public string Author_Name { get; set; } = string.Empty;

//        public ICollection<Book> Books { get; set; } = new List<Book>();
//    }
//}
>>>>>>> f5451a52d1c4c87b33f69c61b45926a525e29c94
