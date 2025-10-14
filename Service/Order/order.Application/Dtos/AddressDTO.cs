namespace order.Application.Dtos
{
    public record AddressDTO
    (
        string FirstName,
        string LastName,
        string Country,
        string Landmark,
        string State,
        string City,
        string PostalCode,
        string Description
        );
}