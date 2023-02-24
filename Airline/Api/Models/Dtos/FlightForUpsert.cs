namespace Api.Models.Dtos;

public class FlightForUpsert
{
    public string Code { get; set; } = default!;
    public decimal Price { get; set; }
}