namespace order.Application.Dtos
{
    public record PaymentDTO(
        string CardNumber,
        string Cvv,
        DateTime ExpiryDate,
        string PaymentType,
        string CardMemberName
        );
    
}
