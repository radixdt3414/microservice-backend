namespace order.Domain.Models
{
    public class Customer : Entity<CustomerId>
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public Customer( CustomerId _Id, string _Name, string _Email)
        {
            Id = _Id;
            Name = _Name;
            Email = _Email;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        public Customer() { }

        public static Customer Create(CustomerId Id, string _Name, string _Email)
        {
            ArgumentException.ThrowIfNullOrEmpty(_Name);
            ArgumentException.ThrowIfNullOrEmpty(_Email);
            var obj = new Customer()
            {
                Name = _Name,
                Email = _Email
            };
            return new Customer(Id, _Name, _Email);
        }
    }
}