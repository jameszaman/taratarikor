namespace Backend.Entities
{
    public class TicketTag
    {
        public int TicketId { get; set; }
        public int TagId { get; set; }

        public Ticket Ticket { get; set; } = default!;
        public Tag Tag { get; set; } = default!;
    }
}
