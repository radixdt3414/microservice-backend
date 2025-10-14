namespace order.Domain.Abstraction
{
    public abstract class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
