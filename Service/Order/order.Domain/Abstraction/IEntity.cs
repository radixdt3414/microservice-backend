namespace order.Domain.Abstraction
{

    public interface IEntity<T> : IEntity
    {
        T Id { get; set; }
    }

    public interface IEntity
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
