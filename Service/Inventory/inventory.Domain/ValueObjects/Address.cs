using System.ComponentModel.DataAnnotations.Schema;

namespace inventory.Domain.ValueObjects
{
    [ComplexType]
    public record Address
    {   
        public string Landmark { get; } = default!;
        public string Country { get; } = default!;
        public string State { get; } = default!;
        public string City { get; } = default!;
        public string PostalCode { get; } = default!;
        public string Description { get; } = default!;

        private Address(string _Country,string _Landmark,string _State,string _City,string _PostalCode,string _Description)
        {
            Country = _Country;
            Landmark = _Landmark;
            State = _State;
            City = _City;
            PostalCode = _PostalCode;
            Description = _Description;
        }

        private Address() { }
        public static Address of(string _Country, string _Landmark, string _State, string _City, string _PostalCode, string _Description)
        {
            ArgumentException.ThrowIfNullOrEmpty(_Country);
            ArgumentException.ThrowIfNullOrEmpty(_Landmark);
            ArgumentException.ThrowIfNullOrEmpty(_State);
            ArgumentException.ThrowIfNullOrEmpty(_City);
            ArgumentException.ThrowIfNullOrEmpty(_PostalCode);
            ArgumentException.ThrowIfNullOrEmpty(_Description);
            
            return new Address( _Country,  _Landmark,  _State,  _City,  _PostalCode,  _Description);
        }
    }
}