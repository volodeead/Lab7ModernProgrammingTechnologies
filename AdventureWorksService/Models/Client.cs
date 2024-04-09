namespace AdventureWorksService.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Навігаційна властивість для квитків
        public ICollection<Ticket> Tickets { get; set; }
    }


}
