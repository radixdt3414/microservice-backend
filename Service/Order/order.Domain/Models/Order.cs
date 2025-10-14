using order.Domain.Events;

namespace order.Domain.Models
{
    public class Order : Aggregate<OrderId>
    {
        
        private readonly List<OrderItem> _orderItems = new List<OrderItem>();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
        public CustomerId CustomerId { get; set; } = default!;
        public Payment Payment { get; set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Address OrderAddress { get; private set; } = default!;
        public OrderName Name { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = default!;
        public decimal TotalPrice { get; private set; } //=> _orderItems.Sum(x => x.Price * x.Quentity);
        //{   
        //    get => _orderItems.Sum(x => x.Price* x.Quentity);
        //    private set;
        ////set => value = _orderItems.Sum(x => x.Price* x.Quentity);
        //}

        //private Order(OrderId _Id,List<OrderItem> _OrderItems,Address _ShippingAddress,Address _OrderAddress,OrderStatus _Status, OrderName _Name)
        //{
        //    Id = _Id;
        //    OrderItems = _OrderItems;
        //    ShippingAddress = _ShippingAddress;
        //    OrderAddress = _OrderAddress;
        //    Status = _Status;
        //    Name = _Name;
        //}

        public static Order Create(OrderId _Id, Address _ShippingAddress, Address _OrderAddress, OrderName _Name,CustomerId CustomerId,Payment Payment, OrderStatus status)
        {
            ArgumentNullException.ThrowIfNull(_ShippingAddress);
            ArgumentNullException.ThrowIfNull(_OrderAddress);
            ArgumentNullException.ThrowIfNull(_Name);
            ArgumentNullException.ThrowIfNull(CustomerId);
            ArgumentNullException.ThrowIfNull(Payment);

            var obj = new Order
            {
                Id = _Id,
                ShippingAddress = _ShippingAddress,
                OrderAddress = _OrderAddress,
                Status = status,
                Name = _Name,
                CustomerId = CustomerId,
                Payment = Payment
            };
            obj.CalculatePrice();
            obj.AddDomainEvents(new OrderCreatedEvent(obj));
            return obj;
        }

        public void CalculatePrice()
        {
            TotalPrice = _orderItems.Sum(x => x.Price * x.Quentity);
        }

        public void Failed()
        {
            CalculatePrice();
            Status = OrderStatus.failed;
        }

        public void Completed()
        {
            CalculatePrice();
            Status = OrderStatus.Completed;
        }

        public void Update(Address _ShippingAddress, Address _OrderAddress, OrderName _Name, CustomerId CustomerId, Payment Payment)
        {
            ArgumentNullException.ThrowIfNull(_ShippingAddress);
            ArgumentNullException.ThrowIfNull(_OrderAddress);
            ArgumentNullException.ThrowIfNull(_Name);
            ArgumentNullException.ThrowIfNull(CustomerId);
            ArgumentNullException.ThrowIfNull(Payment);

            ShippingAddress = _ShippingAddress;
            OrderAddress = _OrderAddress;
            Name = _Name;
            CustomerId = CustomerId;
            Payment = Payment;
            ModifiedDate = DateTime.Now;
            AddDomainEvents(new OrderUpdatedEvent(this));
        }

        public void AddItem(long _Quentity, ProductId _ProductId, decimal _Price)
        {
            _orderItems.Add(OrderItem.Create(Id, _Quentity, _ProductId, _Price));
            AddDomainEvents(new OrderItemAddedEvent(this));
        }

        public void UpdateItem(long _Quentity, Product _Product, decimal _Price)
        {
            var item = _orderItems.Where(x => x.ProductId == _Product.Id).FirstOrDefault();
            if (item != null)
            {
                throw new DomainException("Given item not found in current order.");
            }
            item.Update(_Quentity, _Product.Id, _Price);
            AddDomainEvents(new OrderItemUpdateEvent(this));
        }

        public void RemoveItem(Product _Product)
        {
            var item = _orderItems.Where(x => x.ProductId == _Product.Id).FirstOrDefault();
            if(item != null)
            {
                throw new DomainException("Given item not found in current order.");
            }
            _orderItems.Remove(item!);
            AddDomainEvents(new OrderItemRemoveEvent(this));
        }
    }
}