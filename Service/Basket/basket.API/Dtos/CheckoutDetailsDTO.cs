namespace basket.API.Dtos
{
    public class CheckoutDetailsDTO
    {
        public Guid CustomerId { get; set; } = default!;
        public decimal TotalPrice { get; set; } = default!;
        public string UserName { get; set; } = default!;


        //Payment
        public string CardNumber { get;  set; } = default!;
        public string Cvv { get;  set; } = default!;
        public DateTime ExpiryDate { get; set; } = default!;
        public string PaymentType { get; set; } = default!;
        public string CardMemberName { get; set; } = default!;

        //Shipping address
        public string Shipping_FirstName { get; set; } = default!;
        public string Shipping_LastName { get; set; } = default!;
        public string Shipping_Country { get; set; } = default!;
        public string Shipping_Landmark { get; set; } = default!;
        public string Shipping_State { get; set; } = default!;
        public string Shipping_City { get; set; } = default!;
        public string Shipping_PostalCode { get; set; } = default!;
        public string Shipping_Description { get; set; } = default!;

        //Order address
        public string Order_FirstName { get; set; } = default!;
        public string Order_LastName { get; set; } = default!;
        public string Order_Country { get; set; } = default!;
        public string Order_Landmark { get; set; } = default!;
        public string Order_State { get; set; } = default!;
        public string Order_City { get; set; } = default!;
        public string Order_PostalCode { get; set; } = default!;
        public string Order_Description { get; set; } = default!;
    }
}