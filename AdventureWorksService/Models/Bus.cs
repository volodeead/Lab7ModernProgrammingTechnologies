namespace AdventureWorksService.Models
{
    public class Bus
    {
        public int BusId { get; set; }
        public string LicensePlate { get; set; }
        public string Model { get; set; }
        public int SeatsAvailable { get; set; } // Кількість доступних місць

        // Навігаційна властивість для квитків
        public ICollection<Ticket> Tickets { get; set; }

        public Bus()
        {
            Tickets = new HashSet<Ticket>();
        }

        // Метод для перевірки та продажу квитка
        public bool TrySellTicket(Ticket ticket)
        {
            // Перевіряємо, чи є доступні місця
            if (Tickets.Count < SeatsAvailable)
            {
                Tickets.Add(ticket);
                return true;
            }
            return false;
        }
    }



}
