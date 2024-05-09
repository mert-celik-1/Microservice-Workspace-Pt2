namespace Inbox_Outbox_Order_Outbox_Table_Publisher_Service;

public class OrderOutbox
{
    public Guid IdempotentToken { get; set; }
    public DateTime OccuredOn { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string Type { get; set; }
    public string Payload { get; set; }
}