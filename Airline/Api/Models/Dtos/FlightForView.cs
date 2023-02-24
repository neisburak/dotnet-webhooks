namespace Api.Models.Dtos;

public class FlightForView
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public decimal Price { get; set; }
}