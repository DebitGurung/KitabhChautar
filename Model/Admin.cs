﻿namespace kitabhChauta.Models
{

  
    public class Admin
    {
        public int AdminId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public string PasswordHash { get; internal set; }
    }

}