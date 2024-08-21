namespace Movie_Ticketing.Models
{
    public class UserUpdateModel
    {
        public int Id { get; set; } // Unique identifier for the user
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        //public DateTime MemberSince { get; set; }

        // Add other properties you want to update
    }

}
