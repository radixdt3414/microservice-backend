using buildingBlock.Exceptions;

namespace authentication.API.Exception
{
    public class UserAlreadyExistException : BadRequestException
    {
        public UserAlreadyExistException(string input) : base("UserAlreadyExist",$"User already exist with {input}")
        {
        }
    }
}
