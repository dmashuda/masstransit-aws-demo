namespace Contracts;

public record ProcessOrder
{
    public required Guid OrderId { get; init; }
    public required string CustomerNumber { get; init; }
}