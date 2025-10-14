using System.ComponentModel.DataAnnotations.Schema;

namespace order.Domain.ValueObjects
{
    [ComplexType]
    public class Payment
    {
        public string CardNumber { get; private set; } = default!; 
        public string Cvv { get; private set; }= default!;
        public DateTime ExpiryDate {  get; private set; } = default!;
        public string PaymentType { get; private set; } = default!;
        public string CardMemberName { get; private set; } = default!;
        private Payment() { }
        private Payment(string _CardNumber,string _Cvv,DateTime _ExpiryDate,string _PaymentType, string _CardMemberName) {
            CardNumber = _CardNumber;
            Cvv = _Cvv;
            ExpiryDate = _ExpiryDate;
            PaymentType = _PaymentType;
            CardMemberName = _CardMemberName;
        }

        public static Payment of(string _CardNumber, string _Cvv, DateTime _ExpiryDate, string _PaymentType, string _CardMemberName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(_CardNumber);
            ArgumentException.ThrowIfNullOrWhiteSpace(_Cvv);
            ArgumentNullException.ThrowIfNull(_ExpiryDate);
            ArgumentException.ThrowIfNullOrWhiteSpace(_PaymentType);
            ArgumentException.ThrowIfNullOrWhiteSpace(_CardMemberName);
            
            return new Payment(_CardNumber, _Cvv, _ExpiryDate, _PaymentType, _CardMemberName);
        }

    }
}
