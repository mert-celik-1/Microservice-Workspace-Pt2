using System.ComponentModel.DataAnnotations;

namespace Inbox_Outbox_Stock.API;

public class OrderInbox
{
    [Key]
    public Guid IdempotentToken { get; set; }
    public bool Processed { get; set; }
    public string Payload { get; set; }
}