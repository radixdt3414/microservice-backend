using MassTransit;

namespace StateMachine.StateInstance
{
    public class OrderInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; } = default!;
        public Guid CustomerId { get; set; } = default!;
        public string UserName { get; set; } = default!;

    }
}