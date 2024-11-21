namespace EventAppClient.Pages
{
    public class Event
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public byte[]? Image { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public int TicketTypeId { get; set; }
        public int EventTypeId { get; set; }
        public int? Limit { get; set; }
        public string? EventTypeName { get; set; }
        public string? TicketTypeName { get; set; }
        public bool IsMultiDay { get; set; }
        public bool IsOngoing { get; set; }
        public string? Countdown { get; set; }
        public decimal? TicketPrice { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsSaved { get; set; }

    }
}
