using buildingBlock.Exceptions;

namespace inventory.Application.Exceptions
{
    public class DeliveryNotPossibleException :  NotFoundException
    {
        public DeliveryNotPossibleException():base("Selected address is Out of country, currently we are not providing delivery service here.") { }
    }
}