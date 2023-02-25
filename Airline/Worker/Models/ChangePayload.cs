namespace Worker.Models;

public class ChangePayload
{
    public string FlightCode { get; set; } = default!;
    public decimal NewPrice { get; set; } = default!;
}