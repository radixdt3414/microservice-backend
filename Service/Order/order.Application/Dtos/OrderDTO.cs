using order.Domain.Enum;

namespace order.Application.Dtos
{
    public record OrderDTO
    (
        Guid? Id,
        Guid CustomerId,
        string OrderName,
        List<OrderItemDTO> OrderItems,
        PaymentDTO PaymentDetails,
        AddressDTO OrderAddressDetails,
        AddressDTO ShippingAddressDetails,
        OrderStatus OrderStatus,
        decimal TotalPrice
        );
}