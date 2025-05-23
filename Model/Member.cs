
﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




namespace kitabhChauta.Models
{

    public class Member
    {
        public int MemberId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; } // Made nullable
        public DateTime? DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string ContactNo { get; set; }
        public bool IsStaff { get; set; }
    }





}


   


