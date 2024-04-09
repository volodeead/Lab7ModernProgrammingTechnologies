namespace AdventureWorksService.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }

        // Зовнішні ключі для Client і Bus
        public int ClientId { get; set; }
        public int BusId { get; set; }

        // Навігаційні властивості для Client і Bus
        public Client Client { get; set; }
        public Bus Bus { get; set; }
    }


}
