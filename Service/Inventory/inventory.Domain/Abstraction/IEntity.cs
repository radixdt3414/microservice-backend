namespace inventory.Domain.Abstraction
{

    public abstract class Entity<T> : IEntity<T>, IEntity
    {
        public T Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = default!;
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; } = default!;
    }
    public interface IEntity<T>
    {
        T Id { get; set; } 
    }
    public interface IEntity
    {
        public string? CreatedBy {  get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
