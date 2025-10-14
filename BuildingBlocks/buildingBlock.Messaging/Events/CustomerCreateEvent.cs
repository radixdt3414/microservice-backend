namespace buildingBlock.Messaging.Events
{
    public record CustomerCreateEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
