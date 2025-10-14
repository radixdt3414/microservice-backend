namespace authentication.API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get;set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Salt { get; set; } = default!;
        public string Password { get; set; } = default!;

        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }

    }
}