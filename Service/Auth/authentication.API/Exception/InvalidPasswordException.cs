using buildingBlock.Exceptions;

namespace authentication.API.Exception
{
    public class InvalidPasswordException : BadRequestException
    {
        public InvalidPasswordException() : base("User", "Invalid password")
        {
        }
    }
}
