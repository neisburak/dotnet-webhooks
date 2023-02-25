namespace TravelAgent.Models;

public class Flight
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public decimal Price { get; set; } = default!;
}