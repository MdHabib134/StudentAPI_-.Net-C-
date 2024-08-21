namespace Movie_Ticketing.Models
{
    public class Movie
    {
        public int MovieID { get; set; }
        public String Title { get; set; }
        public String Genre { get; set; }
        public String Description { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
