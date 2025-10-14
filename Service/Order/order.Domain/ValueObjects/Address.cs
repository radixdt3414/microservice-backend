using System.ComponentModel.DataAnnotations.Schema;

namespace order.Domain.ValueObjects
{
    [ComplexType]
    public record Address
    {
        public string FirstName { get; } = default!;
        public string LastName { get; } = default!;
        public string Country { get; } = default!;
        public string Landmark { get; } = default!;
        public string State { get; } = default!;
        public string City { get; } = default!;
        public string PostalCode { get; } = default!;
        public string Description { get; } = default!;

        private Address(string _FirstName,string _LastName,string _Country,string _Landmark,string _State,string _City,string _PostalCode,string _Description)
        {
            FirstName = _FirstName;
            LastName = _LastName;
            Country = _Country;
            Landmark = _Landmark;
            State = _State;
            City = _City;
            PostalCode = _PostalCode;
            Description = _Description;
        }

        private Address() { }
        public static Address of(string _FirstName, string _LastName, string _Country, string _Landmark, string _State, string _City, string _PostalCode, string _Description)
        {
            ArgumentException.ThrowIfNullOrEmpty(_FirstName);
            ArgumentException.ThrowIfNullOrEmpty(_LastName);
            ArgumentException.ThrowIfNullOrEmpty(_Country);
            ArgumentException.ThrowIfNullOrEmpty(_Landmark);
            ArgumentException.ThrowIfNullOrEmpty(_State);
            ArgumentException.ThrowIfNullOrEmpty(_City);
            ArgumentException.ThrowIfNullOrEmpty(_PostalCode);
            ArgumentException.ThrowIfNullOrEmpty(_Description);
            
            return new Address( _FirstName,  _LastName,  _Country,  _Landmark,  _State,  _City,  _PostalCode,  _Description);
        }
    }
}