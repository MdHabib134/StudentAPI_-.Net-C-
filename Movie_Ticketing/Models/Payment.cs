namespace Movie_Ticketing.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public String userEmail { get; set; }
        public String CardHolderName { get; set; }
        public String CardNumber { get; set; }
        public String ExpirationDate { get; set; }
        public String Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
