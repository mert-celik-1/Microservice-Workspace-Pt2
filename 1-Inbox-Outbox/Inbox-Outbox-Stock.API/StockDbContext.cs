using Microsoft.EntityFrameworkCore;

namespace Inbox_Outbox_Stock.API;

public class StockDbContext : DbContext
{
    public StockDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<OrderInbox> OrderInboxes { get; set; }
}