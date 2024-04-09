namespace AdventureWorksService.DTO
{
    public class TicketUpdate
    {
        public int TicketId { get; set; }

        // Зовнішні ключі для Client і Bus
        public int ClientId { get; set; }
        public int BusId { get; set; }


    }


}
