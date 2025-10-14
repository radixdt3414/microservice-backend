using buildingBlock.Exceptions;

namespace authentication.API.Exception
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string input) : base($"User not found with {input}")
        {
        }
    }
}
