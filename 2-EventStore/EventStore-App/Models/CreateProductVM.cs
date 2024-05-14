namespace EventStore_App.Models;

public class CreateProductVM
{
    public string ProductName { get; set; }
    public int Count { get; set; }
    public bool IsAvailable { get; set; }
    public decimal Price { get; set; }
}