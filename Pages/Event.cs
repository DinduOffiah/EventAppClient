namespace EventAppClient.Pages
{
    public class Event
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public byte[]? Image { get; set; }
        public DateTime? EventDate { get; set; }
        public string? Location { get; set; }
        public int TicketTypeId { get; set; }
        public int EventTypeId { get; set; }
        public int? Limit { get; set; }
    }
}
