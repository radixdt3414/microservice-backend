using System.ComponentModel.DataAnnotations.Schema;

namespace order.Domain.ValueObjects
{
    [ComplexType]
    public record OrderName
    {
        public string Value { get; set; } 

        private OrderName(string Val) => Value = Val;

        public static OrderName of(string _name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(_name);
            return new OrderName(_name);
        }
    }
}
