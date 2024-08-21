﻿using System.ComponentModel.DataAnnotations;

namespace Movie_Ticketing.Models
{
    public class User
    {
        public int UserID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Mobile { get; set; }
        public String Password { get; set; }
       public DateTime MemberSince { get; set; }
        
    }
}
