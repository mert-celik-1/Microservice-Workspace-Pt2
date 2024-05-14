namespace EventStore_Shared.Events;

public class CountIncreasedEvent
{
    public string ProductId { get; set; }
    public int IncrementAmount { get; set; }
}