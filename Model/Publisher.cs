
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitabhChauta.Model
{
    public class Publisher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Publisher_id { get; set; }

        [Required(ErrorMessage = "Author name is required")]
        [StringLength(100, ErrorMessage = "Author name cannot be longer than 100 characters")]
        public string Publisher_Name { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}

﻿//using kitabhChautari.Models;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace KitabhChauta.Model
//{
//    public class Publisher
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Publisher_id { get; set; }

//        [Required(ErrorMessage = "Author name is required")]
//        [StringLength(100, ErrorMessage = "Author name cannot be longer than 100 characters")]
//        public string Publisher_Name { get; set; } = string.Empty;

//        public ICollection<Book> Books { get; set; } = new List<Book>();
//    }
//}

