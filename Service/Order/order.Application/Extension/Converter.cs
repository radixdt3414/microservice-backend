using buildingBlock.Messaging.Events;
using order.Domain.Enum;

namespace order.Application.Extension
{
    public static class Converter
    {
        public static IEnumerable<OrderDTO> ModelToDTOConverter(List<OrderModel> lstOrder)
        {
            List<OrderDTO> result = new List<OrderDTO>();
            foreach (var item in lstOrder)
            {
                AddressDTO ShippingAddress = new AddressDTO(
                    item.ShippingAddress.FirstName,
                    item.ShippingAddress.LastName,
                    item.ShippingAddress.Country,
                    item.ShippingAddress.Landmark,
                    item.ShippingAddress.State,
                    item.ShippingAddress.City,
                    item.ShippingAddress.PostalCode,
                    item.ShippingAddress.Description);
                AddressDTO OrderAddress = new AddressDTO(
                    item.OrderAddress.FirstName,
                    item.OrderAddress.LastName,
                    item.OrderAddress.Country,
                    item.OrderAddress.Landmark,
                    item.OrderAddress.State,
                    item.OrderAddress.City,
                    item.OrderAddress.PostalCode,
                    item.OrderAddress.Description);

                PaymentDTO PaymentDetails = new PaymentDTO(
                    item.Payment.CardNumber,
                    item.Payment.Cvv,
                    item.Payment.ExpiryDate,
                    item.Payment.PaymentType,
                    item.Payment.CardMemberName
                    );

                List<OrderItemDTO> orderItemList = new List<OrderItemDTO>();
                foreach (var orderItem in item.OrderItems)
                {
                    orderItemList.Add(new OrderItemDTO(item.Id.Value, orderItem.Quentity, orderItem.ProductId.Value, orderItem.Price, ""));
                }

                OrderDTO obj = new OrderDTO(
                    item.Id.Value,
                    item.CustomerId.Value,
                    item.Name.Value,
                    orderItemList,
                    PaymentDetails,
                    OrderAddress,
                    ShippingAddress,
                    item.Status,
                    item.TotalPrice);
                result.Add(obj);
            }
            return result.AsEnumerable();
        }

        public static OrderDTO EventToDTOConverter(OrderEvent basketObj)
        {
            var orderId = Guid.NewGuid();
            AddressDTO ShippingAddress = new AddressDTO(
                basketObj.Shipping_FirstName ,
                basketObj.Shipping_LastName,
                basketObj.Shipping_Country,
                basketObj.Shipping_Landmark,
                basketObj.Shipping_State,
                basketObj.Shipping_City,
                basketObj.Shipping_PostalCode,
                basketObj.Shipping_Description);
            AddressDTO OrderAddress = new AddressDTO(
                basketObj.Order_FirstName,
                basketObj.Order_LastName,
                basketObj.Order_Country,
                basketObj.Order_Landmark,
                basketObj.Order_State,
                basketObj.Order_City,
                basketObj.Order_PostalCode,
                basketObj.Order_Description);

            PaymentDTO PaymentDetails = new PaymentDTO(
                basketObj.CardNumber,
                basketObj.Cvv,
                basketObj.ExpiryDate,
                basketObj.PaymentType,
                basketObj.CardMemberName
                );

            List<OrderItemDTO> orderItemList = new List<OrderItemDTO>();
            foreach (var orderItem in basketObj.items)
            {
                orderItemList.Add(new OrderItemDTO(null, orderItem.Quentity, orderItem.ProductId, orderItem.Price, ""));
            }

            OrderDTO obj = new OrderDTO(
                null,
                basketObj.CustomerId,
                basketObj.OrderName,
                orderItemList,
                PaymentDetails,
                OrderAddress,
                ShippingAddress,
                OrderStatus.Processing,
                basketObj.TotalPrice);
            return obj;
        }

        public static OrderPlacedEvent DTOToEventConverter(OrderDTO order)
        {
            return new OrderPlacedEvent()
            {
                OrderId = order.Id!.Value,
                CustomerId = order.CustomerId,
                Shipping_Country = order.ShippingAddressDetails.Country,
                Shipping_Landmark = order.ShippingAddressDetails.Landmark,
                Shipping_State = order.ShippingAddressDetails.State,
                Shipping_City = order.ShippingAddressDetails.City,
                Shipping_PostalCode = order.ShippingAddressDetails.PostalCode,
                Shipping_Description = order.ShippingAddressDetails.Description,
                items = order.OrderItems.Select(x => new BaskteItem()
                {
                    Price = x.Price,
                    Quentity = x.Quentity,
                    ProductId = x.ProductId
                }).ToList()
            };
        }
    }
}